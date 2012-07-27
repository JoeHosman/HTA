// Type: Nest.ElasticClient
// Assembly: Nest, Version=0.9.2.0, Culture=neutral, PublicKeyToken=null
// Assembly location: E:\Dev\git\HTA\src\HTA\packages\NEST.0.9.2.0\lib\NET4\Nest.dll

using Nest.Resolvers;
using Nest.Resolvers.Converters;
using Nest.Resolvers.Writers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nest
{
  public class ElasticClient : IElasticClient
  {
    private static Regex StripIndex = new Regex("^index\\.");
    private static ConcurrentDictionary<Type, Func<object, string>> IdDelegates = new ConcurrentDictionary<Type, Func<object, string>>();
    internal static readonly JsonSerializerSettings DeserializeSettings = ElasticClient.CreateDeserializeSettings();
    internal static readonly JsonSerializerSettings SerializationSettings = ElasticClient.CreateSettings();
    public static readonly PropertyNameResolver PropertyNameResolver = new PropertyNameResolver(ElasticClient.SerializationSettings);
    private readonly string _aliasBody;
    private Regex _bulkReplace;
    private bool _gotNodeInfo;

    private IConnection Connection { get; set; }

    private IConnectionSettings Settings { get; set; }

    private bool _IsValid { get; set; }

    private ElasticSearchVersionInfo _VersionInfo { get; set; }

    public bool IsValid
    {
      get
      {
        if (!this._gotNodeInfo)
          this.GetNodeInfo();
        return this._IsValid;
      }
    }

    public ElasticSearchVersionInfo VersionInfo
    {
      get
      {
        if (!this._gotNodeInfo)
          this.GetNodeInfo();
        return this._VersionInfo;
      }
    }

    static ElasticClient()
    {
    }

    public ElasticClient(IConnectionSettings settings)
      : this(settings, (IConnection) new Connection(settings))
    {
    }

    public ElasticClient(IConnectionSettings settings, IConnection connection)
    {
      if (settings == null)
        throw new ArgumentNullException("settings");
      this.Settings = settings;
      this.Connection = connection;
    }

    public UpdateResponse Update<T>(Action<UpdateDescriptor<T>> updateSelector) where T : class
    {
      UpdateDescriptor<T> updateDescriptor = new UpdateDescriptor<T>();
      updateSelector(updateDescriptor);
      string data = ElasticClient.Serialize<UpdateDescriptor<T>>(updateDescriptor);
      return this._Update(this.CreateUpdatePath<T>(updateDescriptor), data);
    }

    public UpdateResponse Update(Action<UpdateDescriptor<object>> updateSelector)
    {
      UpdateDescriptor<object> updateDescriptor = new UpdateDescriptor<object>();
      updateSelector(updateDescriptor);
      string data = ElasticClient.Serialize<UpdateDescriptor<object>>(updateDescriptor);
      return this._Update(this.CreateUpdatePath<object>(updateDescriptor), data);
    }

    private string CreateUpdatePath<T>(UpdateDescriptor<T> s) where T : class
    {
      string str1 = s._Index ?? this.Settings.GetIndexForType<T>();
      string str2 = s._Type ?? ElasticClient.GetTypeNameFor<T>();
      string str3 = s._Id ?? this.GetIdFor<T>(s._Object);
      Nest.Extensions.ThrowIfNullOrEmpty(str1, "index");
      Nest.Extensions.ThrowIfNullOrEmpty(str2, "type");
      Nest.Extensions.ThrowIfNullOrEmpty(str3, "id");
      string str4 = this.CreatePath(str1, str2, str3) + "/_update?";
      if (s._Consistency.HasValue)
        str4 = str4 + "consistency=" + Enum.GetName(typeof (Consistency), (object) s._Consistency.Value);
      if (s._Replication.HasValue)
        str4 = str4 + "replication=" + Enum.GetName(typeof (Replication), (object) s._Replication.Value);
      if (s._Refresh.HasValue)
        str4 = str4 + "refresh=" + s._Refresh.Value.ToString().ToLower();
      if (s._Timeout.HasValue)
        str4 = str4 + "timeout=" + s._Timeout.ToString();
      if (s._Timeout.HasValue)
        str4 = str4 + "timeout=" + s._Timeout.ToString();
      if (!string.IsNullOrWhiteSpace(s._Percolate))
        str4 = str4 + "percolate=" + s._Percolate;
      if (!string.IsNullOrWhiteSpace(s._Parent))
        str4 = str4 + "parent=" + s._Parent;
      if (!string.IsNullOrWhiteSpace(s._Routing))
        str4 = str4 + "routing=" + s._Routing;
      return str4;
    }

    private UpdateResponse _Update(string path, string data)
    {
      return this.ToParsedResponse<UpdateResponse>(this.Connection.PostSync(path, data), false);
    }

    private static JsonSerializerSettings CreateSettings()
    {
      return new JsonSerializerSettings()
      {
        ContractResolver = (IContractResolver) new ElasticResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore,
        Converters = (IList<JsonConverter>) new List<JsonConverter>()
        {
          (JsonConverter) new IsoDateTimeConverter(),
          (JsonConverter) new TermConverter(),
          (JsonConverter) new FacetConverter(),
          (JsonConverter) new IndexSettingsConverter(),
          (JsonConverter) new ShardsSegmentConverter(),
          (JsonConverter) new RawOrQueryDescriptorConverter()
        }
      };
    }

    private static JsonSerializerSettings CreateDeserializeSettings()
    {
      return new JsonSerializerSettings()
      {
        ContractResolver = (IContractResolver) new ElasticResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore,
        Converters = (IList<JsonConverter>) new List<JsonConverter>()
        {
          (JsonConverter) new IsoDateTimeConverter(),
          (JsonConverter) new FacetConverter(),
          (JsonConverter) new ShardsSegmentConverter()
        }
      };
    }

    public static void AddConverter(JsonConverter converter)
    {
      ElasticClient.SerializationSettings.Converters.Add(converter);
      ElasticClient.DeserializeSettings.Converters.Add(converter);
    }

    public static string Serialize<T>(T @object)
    {
      return JsonConvert.SerializeObject((object) @object, Formatting.Indented, ElasticClient.SerializationSettings);
    }

    public static T Deserialize<T>(string value)
    {
      return JsonConvert.DeserializeObject<T>(value, ElasticClient.DeserializeSettings);
    }

    public RegisterPercolateResponse RegisterPercolator(string name, Action<QueryPathDescriptor<object>> querySelector)
    {
      return this.RegisterPercolator<object>(name, querySelector);
    }

    public RegisterPercolateResponse RegisterPercolator<T>(string name, Action<QueryPathDescriptor<T>> querySelector) where T : class
    {
      Nest.Extensions.ThrowIfNull<Action<QueryPathDescriptor<T>>>(querySelector, "queryDescriptor");
      QueryPathDescriptor<T> queryPathDescriptor = new QueryPathDescriptor<T>();
      querySelector(queryPathDescriptor);
      string query = ElasticClient.Serialize(new
      {
        query = queryPathDescriptor
      });
      string stringToEscape = this.Settings.GetIndexForType<T>();
      if (Nest.Extensions.HasAny<string>(queryPathDescriptor._Indices))
        stringToEscape = Enumerable.First<string>(queryPathDescriptor._Indices);
      return this._RegisterPercolator(Nest.Extensions.F("_percolator/{0}/{1}", (object) Uri.EscapeDataString(stringToEscape), (object) Uri.EscapeDataString(name)), query);
    }

    [Obsolete("Passing a query by string? Found a bug in the DSL? https://github.com/Mpdreamz/NEST/issues")]
    public RegisterPercolateResponse RegisterPercolator(string index, string name, string query)
    {
      return this._RegisterPercolator(Nest.Extensions.F("_percolator/{0}/{1}", (object) Uri.EscapeDataString(index), (object) Uri.EscapeDataString(name)), query);
    }

    private RegisterPercolateResponse _RegisterPercolator(string path, string query)
    {
      return this.ToParsedResponse<RegisterPercolateResponse>(this.Connection.PutSync(path, query), false);
    }

    public UnregisterPercolateResponse UnregisterPercolator<T>(string name) where T : class
    {
      return this.UnregisterPercolator(this.Settings.GetIndexForType<T>(), name);
    }

    public UnregisterPercolateResponse UnregisterPercolator(string index, string name)
    {
      return this._UnregisterPercolator(Nest.Extensions.F("_percolator/{0}/{1}", (object) Uri.EscapeDataString(index), (object) Uri.EscapeDataString(name)));
    }

    private UnregisterPercolateResponse _UnregisterPercolator(string path)
    {
      return this.ToParsedResponse<UnregisterPercolateResponse>(this.Connection.DeleteSync(path), true);
    }

    public PercolateResponse Percolate<T>(T @object) where T : class
    {
      return this.Percolate(this.Settings.GetIndexForType<T>(), this.InferTypeName<T>(), Nest.Extensions.F("{{doc:{0}}}", new object[1]
      {
        (object) JsonConvert.SerializeObject((object) @object, Formatting.Indented, ElasticClient.SerializationSettings)
      }));
    }

    public PercolateResponse Percolate<T>(string index, T @object) where T : class
    {
      string type = this.InferTypeName<T>();
      string str = JsonConvert.SerializeObject((object) @object, Formatting.Indented, ElasticClient.SerializationSettings);
      return this.Percolate(index, type, Nest.Extensions.F("{{doc:{0}}}", new object[1]
      {
        (object) str
      }));
    }

    public PercolateResponse Percolate<T>(string index, string type, T @object) where T : class
    {
      string str = JsonConvert.SerializeObject((object) @object, Formatting.Indented, ElasticClient.SerializationSettings);
      return this.Percolate(index, type, Nest.Extensions.F("{{doc:{0}}}", new object[1]
      {
        (object) str
      }));
    }

    private PercolateResponse Percolate(string index, string type, string doc)
    {
      return this.ToParsedResponse<PercolateResponse>(this.Connection.PostSync(this.CreatePath(index, type) + "_percolate", doc), false);
    }

    public SegmentsResponse Segments()
    {
      return this._Segments("_segments");
    }

    public SegmentsResponse Segments(string index)
    {
      return this.Segments((IEnumerable<string>) new string[1]
      {
        index
      });
    }

    public SegmentsResponse Segments(IEnumerable<string> indices)
    {
      return this._Segments(this.CreatePath(string.Join(",", indices)) + "_segments");
    }

    private SegmentsResponse _Segments(string path)
    {
      return this.ToParsedResponse<SegmentsResponse>(this.Connection.GetSync(path), false);
    }

    public IndexExistsResponse IndexExists(string index)
    {
      return this._IndexExists(index);
    }

    private IndexExistsResponse _IndexExists(string index)
    {
      ConnectionStatus connectionStatus = this.Connection.HeadSync(this.CreatePath(index));
      IndexExistsResponse indexExistsResponse1 = new IndexExistsResponse();
      indexExistsResponse1.IsValid = false;
      indexExistsResponse1.Exists = false;
      indexExistsResponse1.ConnectionStatus = connectionStatus;
      IndexExistsResponse indexExistsResponse2 = indexExistsResponse1;
      if (connectionStatus.Error == null || connectionStatus.Error.HttpStatusCode == HttpStatusCode.NotFound)
        indexExistsResponse2.IsValid = true;
      if (connectionStatus.Error == null)
        indexExistsResponse2.Exists = true;
      return indexExistsResponse2;
    }

    private string _BuildStatsUrl(StatsParams parameters)
    {
      string str1 = "_stats";
      if (parameters == null)
        return str1;
      StatsInfo infoOn = parameters.InfoOn;
      if (infoOn != StatsInfo.None)
      {
        string str2 = str1 + "?clear=true";
        bool flag = (infoOn & (StatsInfo.Docs | StatsInfo.Store | StatsInfo.Indexing | StatsInfo.Get | StatsInfo.Search | StatsInfo.Merge | StatsInfo.Flush)) == (StatsInfo.Docs | StatsInfo.Store | StatsInfo.Indexing | StatsInfo.Get | StatsInfo.Search | StatsInfo.Merge | StatsInfo.Flush);
        List<string> list = new List<string>();
        if (flag || (infoOn & StatsInfo.Docs) == StatsInfo.Docs)
          list.Add("docs=true");
        if (flag || (infoOn & StatsInfo.Store) == StatsInfo.Store)
          list.Add("store=true");
        if (flag || (infoOn & StatsInfo.Indexing) == StatsInfo.Indexing)
          list.Add("indexing=true");
        if (flag || (infoOn & StatsInfo.Get) == StatsInfo.Get)
          list.Add("get=true");
        if (flag || (infoOn & StatsInfo.Search) == StatsInfo.Search)
          list.Add("search=true");
        if (flag || (infoOn & StatsInfo.Merge) == StatsInfo.Merge)
          list.Add("merge=true");
        if (flag || (infoOn & StatsInfo.Flush) == StatsInfo.Flush)
          list.Add("flush=true");
        str1 = str2 + "&" + string.Join("&", (IEnumerable<string>) list);
      }
      if (parameters.Refresh)
        str1 = str1 + "&refresh=true";
      if (parameters.Types != null && Enumerable.Any<string>((IEnumerable<string>) parameters.Types))
        str1 = str1 + "&types=" + string.Join(",", (IEnumerable<string>) parameters.Types);
      if (parameters.Groups != null && Enumerable.Any<string>((IEnumerable<string>) parameters.Groups))
        str1 = str1 + "&groups=" + string.Join(",", (IEnumerable<string>) parameters.Groups);
      return str1;
    }

    public GlobalStatsResponse Stats()
    {
      return this.Stats(new StatsParams());
    }

    public GlobalStatsResponse Stats(StatsParams parameters)
    {
      return this.ToParsedResponse<GlobalStatsResponse>(this.Connection.GetSync(this._BuildStatsUrl(parameters)), false);
    }

    public StatsResponse Stats(IEnumerable<string> indices)
    {
      Nest.Extensions.ThrowIfEmpty<string>(indices, "indices");
      return this.Stats(indices, new StatsParams());
    }

    public StatsResponse Stats(string index)
    {
      Nest.Extensions.ThrowIfNullOrEmpty(index, "index");
      return this.Stats((IEnumerable<string>) new string[1]
      {
        index
      }, new StatsParams());
    }

    public StatsResponse Stats(IEnumerable<string> indices, StatsParams parameters)
    {
      Nest.Extensions.ThrowIfEmpty<string>(indices, "indices");
      return this.ToParsedResponse<StatsResponse>(this.Connection.GetSync(this.CreatePath(string.Join(",", indices)) + this._BuildStatsUrl(parameters)), false);
    }

    public IndexSettingsResponse GetIndexSettings()
    {
      return this.GetIndexSettings(this.Settings.DefaultIndex);
    }

    public IndexSettingsResponse GetIndexSettings(string index)
    {
      ConnectionStatus sync = this.Connection.GetSync(this.CreatePath(index) + "_settings");
      IndexSettingsResponse settingsResponse = new IndexSettingsResponse();
      settingsResponse.IsValid = false;
      try
      {
        JToken first = JObject.Parse(sync.Result).First.First.First.First;
        IndexSettings indexSettings = JsonConvert.DeserializeObject<IndexSettings>(((object) first).ToString());
        foreach (JProperty jproperty in first.Children<JProperty>())
          indexSettings.Add(ElasticClient.StripIndex.Replace(jproperty.Name, ""), ((object) jproperty.Value).ToString());
        settingsResponse.Settings = indexSettings;
        settingsResponse.IsValid = true;
      }
      catch
      {
      }
      settingsResponse.ConnectionStatus = sync;
      return settingsResponse;
    }

    public SettingsOperationResponse UpdateSettings(IndexSettings settings)
    {
      return this.UpdateSettings(this.Settings.DefaultIndex, settings);
    }

    public SettingsOperationResponse UpdateSettings(string index, IndexSettings settings)
    {
      string path = this.CreatePath(index) + "_settings";
      settings.Settings = Enumerable.ToDictionary<KeyValuePair<string, string>, string, string>(Enumerable.Where<KeyValuePair<string, string>>((IEnumerable<KeyValuePair<string, string>>) settings.Settings, (Func<KeyValuePair<string, string>, bool>) (kv => Enumerable.Any<string>(IndexSettings.UpdateWhiteList, (Func<string, bool>) (p => kv.Key.StartsWith(p))))), (Func<KeyValuePair<string, string>, string>) (kv => kv.Key), (Func<KeyValuePair<string, string>, string>) (kv => kv.Value));
      StringBuilder sb = new StringBuilder();
      using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) new StringWriter(sb)))
      {
        jsonWriter.WriteStartObject();
        jsonWriter.WritePropertyName("index");
        jsonWriter.WriteStartObject();
        foreach (KeyValuePair<string, string> keyValuePair in settings.Settings)
        {
          jsonWriter.WritePropertyName(keyValuePair.Key);
          jsonWriter.WriteValue(keyValuePair.Value);
        }
        jsonWriter.WriteEndObject();
      }
      string data = ((object) sb).ToString();
      ConnectionStatus connectionStatus = this.Connection.PutSync(path, data);
      SettingsOperationResponse operationResponse = new SettingsOperationResponse();
      try
      {
        operationResponse = JsonConvert.DeserializeObject<SettingsOperationResponse>(connectionStatus.Result);
      }
      catch
      {
      }
      operationResponse.IsValid = connectionStatus.Success;
      operationResponse.ConnectionStatus = connectionStatus;
      return operationResponse;
    }

    public IndicesResponse CreateIndex(string index, IndexSettings settings)
    {
      ConnectionStatus connectionStatus = this.Connection.PostSync(this.CreatePath(index), JsonConvert.SerializeObject((object) settings, Formatting.None, ElasticClient.SerializationSettings));
      IndicesResponse indicesResponse = new IndicesResponse();
      indicesResponse.ConnectionStatus = connectionStatus;
      try
      {
        indicesResponse = JsonConvert.DeserializeObject<IndicesResponse>(connectionStatus.Result);
        indicesResponse.IsValid = true;
      }
      catch
      {
      }
      return indicesResponse;
    }

    public IndicesResponse DeleteIndex<T>() where T : class
    {
      return this.DeleteIndex(this.Settings.GetIndexForType<T>());
    }

    public IndicesResponse DeleteIndex(string index)
    {
      return this.ToParsedResponse<IndicesResponse>(this.Connection.DeleteSync(this.CreatePath(index)), false);
    }

    public IndicesResponse ClearCache()
    {
      return this.ClearCache((List<string>) null, ClearCacheOptions.Id | ClearCacheOptions.Filter | ClearCacheOptions.FieldData | ClearCacheOptions.Bloom);
    }

    public IndicesResponse ClearCache<T>() where T : class
    {
      return this.ClearCache(new List<string>()
      {
        this.Settings.GetIndexForType<T>()
      }, ClearCacheOptions.Id | ClearCacheOptions.Filter | ClearCacheOptions.FieldData | ClearCacheOptions.Bloom);
    }

    public IndicesResponse ClearCache<T>(ClearCacheOptions options) where T : class
    {
      return this.ClearCache(new List<string>()
      {
        this.Settings.GetIndexForType<T>()
      }, options);
    }

    public IndicesResponse ClearCache(ClearCacheOptions options)
    {
      return this.ClearCache((List<string>) null, options);
    }

    public IndicesResponse ClearCache(List<string> indices, ClearCacheOptions options)
    {
      string path = "/_cache/clear";
      if (indices != null && Enumerable.Any<string>((IEnumerable<string>) indices, (Func<string, bool>) (s => !string.IsNullOrEmpty(s))))
        path = "/" + string.Join(",", Enumerable.ToArray<string>(Enumerable.Where<string>((IEnumerable<string>) indices, (Func<string, bool>) (s => !string.IsNullOrEmpty(s))))) + path;
      if (options != (ClearCacheOptions.Id | ClearCacheOptions.Filter | ClearCacheOptions.FieldData | ClearCacheOptions.Bloom))
      {
        List<string> list = new List<string>();
        if ((options & ClearCacheOptions.Id) == ClearCacheOptions.Id)
          list.Add("id=true");
        if ((options & ClearCacheOptions.Filter) == ClearCacheOptions.Filter)
          list.Add("filter=true");
        if ((options & ClearCacheOptions.FieldData) == ClearCacheOptions.FieldData)
          list.Add("field_data=true");
        if ((options & ClearCacheOptions.Bloom) == ClearCacheOptions.Bloom)
          list.Add("bloom=true");
        path = path + "?" + string.Join("&", list.ToArray());
      }
      ConnectionStatus connectionStatus = this.Connection.PostSync(path, string.Empty);
      IndicesResponse indicesResponse = new IndicesResponse();
      try
      {
        indicesResponse = JsonConvert.DeserializeObject<IndicesResponse>(connectionStatus.Result);
        indicesResponse.IsValid = true;
      }
      catch
      {
      }
      indicesResponse.ConnectionStatus = connectionStatus;
      return indicesResponse;
    }

    public AnalyzeResponse Analyze(string text)
    {
      string defaultIndex = this.Settings.DefaultIndex;
      return this._Analyze(new AnalyzeParams()
      {
        Index = defaultIndex
      }, text);
    }

    public AnalyzeResponse Analyze(AnalyzeParams analyzeParams, string text)
    {
      Nest.Extensions.ThrowIfNull<AnalyzeParams>(analyzeParams, "analyzeParams");
      Nest.Extensions.ThrowIfNull<string>(analyzeParams.Index, "analyzeParams.Index");
      return this._Analyze(analyzeParams, text);
    }

    public AnalyzeResponse Analyze<T>(Expression<Func<T, object>> selector, string text) where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      return this.Analyze<T>(selector, indexForType, text);
    }

    public AnalyzeResponse Analyze<T>(Expression<Func<T, object>> selector, string index, string text) where T : class
    {
      Nest.Extensions.ThrowIfNull<Expression<Func<T, object>>>(selector, "selector");
      string str = ElasticClient.PropertyNameResolver.Resolve((Expression) selector);
      return this._Analyze(new AnalyzeParams()
      {
        Index = index,
        Field = str
      }, text);
    }

    private AnalyzeResponse _Analyze(AnalyzeParams analyzeParams, string text)
    {
      string path = this.CreatePath(analyzeParams.Index) + "_analyze?text=" + Uri.EscapeDataString(text);
      if (!Nest.Extensions.IsNullOrEmpty(analyzeParams.Field))
        path = path + "&field=" + analyzeParams.Field;
      else if (!Nest.Extensions.IsNullOrEmpty(analyzeParams.Analyzer))
        path = path + "&analyzer=" + analyzeParams.Analyzer;
      return this.ToParsedResponse<AnalyzeResponse>(this.Connection.GetSync(path), false);
    }

    private string _createCommand(string command, AliasParams aliasParam)
    {
      string str = Nest.Extensions.F("{{ \"{0}\" : {{\n\t\t\t\tindex: \"{1}\",\n\t\t\t\talias: \"{2}\"", (object) command, (object) aliasParam.Index, (object) aliasParam.Alias);
      if (!Nest.Extensions.IsNullOrEmpty(aliasParam.Filter))
        str = str + Nest.Extensions.F(", \"filter\": {0} ", new object[1]
        {
          (object) aliasParam.Filter
        });
      if (!Nest.Extensions.IsNullOrEmpty(aliasParam.Routing))
      {
        str = str + Nest.Extensions.F(", \"routing\": \"{0}\" ", new object[1]
        {
          (object) aliasParam.Routing
        });
      }
      else
      {
        if (!Nest.Extensions.IsNullOrEmpty(aliasParam.IndexRouting))
          str = str + Nest.Extensions.F(", \"index_routing\": \"{0}\" ", new object[1]
          {
            (object) aliasParam.IndexRouting
          });
        if (!Nest.Extensions.IsNullOrEmpty(aliasParam.SearchRouting))
          str = str + Nest.Extensions.F(", \"search_routing\": \"{0}\" ", new object[1]
          {
            (object) aliasParam.SearchRouting
          });
      }
      return str + "} }";
    }

    public IEnumerable<string> GetIndicesPointingToAlias(string alias)
    {
      Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(this.Connection.GetSync(this.CreatePath(alias) + "/_aliases").Result, ElasticClient.DeserializeSettings);
      if (dictionary != null)
        return (IEnumerable<string>) dictionary.Keys;
      else
        return Enumerable.Empty<string>();
    }

    public IndicesOperationResponse Swap(string alias, IEnumerable<string> oldIndices, IEnumerable<string> newIndices)
    {
      List<string> list = new List<string>();
      foreach (string str in oldIndices)
        list.Add(this._createCommand("remove", new AliasParams()
        {
          Index = str,
          Alias = alias
        }));
      foreach (string str in newIndices)
        list.Add(this._createCommand("add", new AliasParams()
        {
          Index = str,
          Alias = alias
        }));
      return this._Alias(string.Join(", ", (IEnumerable<string>) list));
    }

    public IndicesOperationResponse Alias(string alias)
    {
      string defaultIndex = this.Settings.DefaultIndex;
      return this._Alias(this._createCommand("add", new AliasParams()
      {
        Index = defaultIndex,
        Alias = alias
      }));
    }

    public IndicesOperationResponse Alias(string index, string alias)
    {
      return this._Alias(this._createCommand("add", new AliasParams()
      {
        Index = index,
        Alias = alias
      }));
    }

    public IndicesOperationResponse Alias(string index, IEnumerable<string> aliases)
    {
      Enumerable.Select<string, string>(aliases, (Func<string, string>) (a => this._createCommand("add", new AliasParams()
      {
        Index = index,
        Alias = a
      })));
      return this._Alias(string.Join(",", aliases));
    }

    public IndicesOperationResponse Alias(IEnumerable<string> aliases)
    {
      string index = this.Settings.DefaultIndex;
      Enumerable.Select<string, string>(aliases, (Func<string, string>) (a => this._createCommand("add", new AliasParams()
      {
        Index = index,
        Alias = a
      })));
      return this._Alias(string.Join(",", aliases));
    }

    public IndicesOperationResponse RemoveAlias(string alias)
    {
      string defaultIndex = this.Settings.DefaultIndex;
      return this._Alias(this._createCommand("remove", new AliasParams()
      {
        Index = defaultIndex,
        Alias = alias
      }));
    }

    public IndicesOperationResponse RemoveAlias(string index, string alias)
    {
      return this._Alias(this._createCommand("remove", new AliasParams()
      {
        Index = index,
        Alias = alias
      }));
    }

    public IndicesOperationResponse RemoveAlias(IEnumerable<string> aliases)
    {
      string index = this.Settings.DefaultIndex;
      Enumerable.Select<string, string>(aliases, (Func<string, string>) (a => this._createCommand("remove", new AliasParams()
      {
        Index = index,
        Alias = a
      })));
      return this._Alias(string.Join(",", aliases));
    }

    public IndicesOperationResponse RemoveAlias(string index, IEnumerable<string> aliases)
    {
      Enumerable.Select<string, string>(aliases, (Func<string, string>) (a => this._createCommand("remove", new AliasParams()
      {
        Index = index,
        Alias = a
      })));
      return this._Alias(string.Join(",", aliases));
    }

    public IndicesOperationResponse Alias(IEnumerable<string> indices, string alias)
    {
      Enumerable.Select<string, string>(indices, (Func<string, string>) (i => this._createCommand("add", new AliasParams()
      {
        Index = i,
        Alias = alias
      })));
      return this._Alias(string.Join(",", indices));
    }

    public IndicesOperationResponse Rename(string index, string oldAlias, string newAlias)
    {
      return this._Alias(this._createCommand("remove", new AliasParams()
      {
        Index = index,
        Alias = oldAlias
      }) + ", " + this._createCommand("add", new AliasParams()
      {
        Index = index,
        Alias = newAlias
      }));
    }

    public IndicesOperationResponse Alias(AliasParams aliasParams)
    {
      return this._Alias(this._createCommand("add", aliasParams));
    }

    public IndicesOperationResponse Alias(IEnumerable<AliasParams> aliases)
    {
      Enumerable.Select<AliasParams, string>(aliases, (Func<AliasParams, string>) (a => Nest.Extensions.F(this._aliasBody, new object[1]
      {
        (object) this._createCommand("add", a)
      })));
      return this._Alias(string.Join<AliasParams>(",", aliases));
    }

    public IndicesOperationResponse RemoveAlias(AliasParams aliasParams)
    {
      return this._Alias(this._createCommand("remove", aliasParams));
    }

    public IndicesOperationResponse RemoveAliases(IEnumerable<AliasParams> aliases)
    {
      Enumerable.Select<AliasParams, string>(aliases, (Func<AliasParams, string>) (a => Nest.Extensions.F(this._aliasBody, new object[1]
      {
        (object) this._createCommand("remove", a)
      })));
      return this._Alias(string.Join<AliasParams>(",", aliases));
    }

    private IndicesOperationResponse _Alias(string query)
    {
      string path = "/_aliases";
      query = Nest.Extensions.F(this._aliasBody, new object[1]
      {
        (object) query
      });
      return this.ToParsedResponse<IndicesOperationResponse>(this.Connection.PostSync(path, query), false);
    }

    public IndicesOperationResponse Optimize<T>() where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      return this.Optimize(indexForType);
    }

    public IndicesOperationResponse Optimize<T>(OptimizeParams optimizeParameters) where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      return this.Optimize(indexForType, optimizeParameters);
    }

    public IndicesOperationResponse Optimize(string index)
    {
      Nest.Extensions.ThrowIfNull<string>(index, "index");
      return this.Optimize((IEnumerable<string>) new string[1]
      {
        index
      });
    }

    public IndicesOperationResponse Optimize(string index, OptimizeParams optimizeParameters)
    {
      Nest.Extensions.ThrowIfNull<string>(index, "index");
      return this.Optimize((IEnumerable<string>) new string[1]
      {
        index
      }, optimizeParameters);
    }

    public IndicesOperationResponse Optimize()
    {
      return this.Optimize("_all");
    }

    public IndicesOperationResponse Optimize(OptimizeParams optimizeParameters)
    {
      return this.Optimize("_all", optimizeParameters);
    }

    public IndicesOperationResponse Optimize(IEnumerable<string> indices)
    {
      Nest.Extensions.ThrowIfNull<IEnumerable<string>>(indices, "index");
      return this._Optimize(this.CreatePath(string.Join(",", indices)) + "_optimize", (OptimizeParams) null);
    }

    public IndicesOperationResponse Optimize(IEnumerable<string> indices, OptimizeParams optimizeParameters)
    {
      Nest.Extensions.ThrowIfNull<IEnumerable<string>>(indices, "index");
      return this._Optimize(this.CreatePath(string.Join(",", indices)) + "_optimize", optimizeParameters);
    }

    private IndicesOperationResponse _Optimize(string path, OptimizeParams optimizeParameters)
    {
      if (optimizeParameters != null)
      {
        path = path + (object) "?max_num_segments=" + (string) (object) optimizeParameters.MaximumSegments;
        path = path + "&only_expunge_deletes=" + optimizeParameters.OnlyExpungeDeletes.ToString().ToLower();
        path = path + "&refresh=" + optimizeParameters.Refresh.ToString().ToLower();
        path = path + "&flush=" + optimizeParameters.Flush.ToString().ToLower();
        path = path + "&wait_for_merge=" + optimizeParameters.WaitForMerge.ToString().ToLower();
      }
      return this.ToParsedResponse<IndicesOperationResponse>(this.Connection.PostSync(path, ""), false);
    }

    public IndicesShardResponse Snapshot()
    {
      return this.Snapshot("_all");
    }

    public IndicesShardResponse Snapshot<T>() where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      return this.Snapshot(indexForType);
    }

    public IndicesShardResponse Snapshot(string index)
    {
      Nest.Extensions.ThrowIfNull<string>(index, "index");
      return this.Snapshot((IEnumerable<string>) new string[1]
      {
        index
      });
    }

    public IndicesShardResponse Snapshot(IEnumerable<string> indices)
    {
      Nest.Extensions.ThrowIfNull<IEnumerable<string>>(indices, "indices");
      return this._Snapshot(this.CreatePath(string.Join(",", indices)) + "_gateway/snapshot");
    }

    private IndicesShardResponse _Snapshot(string path)
    {
      return this.ToParsedResponse<IndicesShardResponse>(this.Connection.PostSync(path, ""), false);
    }

    public IndicesOperationResponse Flush<T>(bool refresh = false) where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      return this.Flush(indexForType, refresh);
    }

    public IndicesOperationResponse Flush(bool refresh = false)
    {
      return this.Flush("_all", refresh);
    }

    public IndicesOperationResponse Flush(string index, bool refresh = false)
    {
      Nest.Extensions.ThrowIfNull<string>(index, "index");
      return this.Flush((IEnumerable<string>) new string[1]
      {
        index
      }, (refresh ? 1 : 0) != 0);
    }

    public IndicesOperationResponse Flush(IEnumerable<string> indices, bool refresh = false)
    {
      Nest.Extensions.ThrowIfNull<IEnumerable<string>>(indices, "index");
      return this._Flush(this.CreatePath(string.Join(",", indices)) + "_flush?refresh=" + refresh.ToString().ToLower());
    }

    private IndicesOperationResponse _Flush(string path)
    {
      return this.ToParsedResponse<IndicesOperationResponse>(this.Connection.PostSync(path, ""), false);
    }

    public IndicesOperationResponse OpenIndex(string index)
    {
      return this._OpenClose(this.CreatePath(index) + "_open");
    }

    public IndicesOperationResponse CloseIndex(string index)
    {
      return this._OpenClose(this.CreatePath(index) + "_close");
    }

    public IndicesOperationResponse OpenIndex<T>() where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      return this.OpenIndex(indexForType);
    }

    public IndicesOperationResponse CloseIndex<T>() where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      return this.CloseIndex(indexForType);
    }

    private IndicesOperationResponse _OpenClose(string path)
    {
      return this.ToParsedResponse<IndicesOperationResponse>(this.Connection.PostSync(path, ""), false);
    }

    public IndicesShardResponse Refresh()
    {
      return this.Refresh("_all");
    }

    public IndicesShardResponse Refresh(string index)
    {
      Nest.Extensions.ThrowIfNull<string>(index, "index");
      return this.Refresh((IEnumerable<string>) new string[1]
      {
        index
      });
    }

    public IndicesShardResponse Refresh(IEnumerable<string> indices)
    {
      Nest.Extensions.ThrowIfNull<IEnumerable<string>>(indices, "indices");
      return this._Refresh(this.CreatePath(string.Join(",", indices)) + "_refresh");
    }

    public IndicesShardResponse Refresh<T>() where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      return this.Refresh(indexForType);
    }

    private IndicesShardResponse _Refresh(string path)
    {
      return this.ToParsedResponse<IndicesShardResponse>(this.Connection.GetSync(path), false);
    }

    [Obsolete("Passing a query by string? Found a bug in the DSL? https://github.com/Mpdreamz/NEST/issues")]
    public CountResponse CountAll(string query)
    {
      return this._Count("_count", query);
    }

    public CountResponse CountAll(Action<QueryDescriptor> querySelector)
    {
      Nest.Extensions.ThrowIfNull<Action<QueryDescriptor>>(querySelector, "querySelector");
      QueryDescriptor @object = new QueryDescriptor();
      querySelector(@object);
      return this._Count("_count", ElasticClient.Serialize<QueryDescriptor>(@object));
    }

    public CountResponse CountAll<T>(Action<QueryDescriptor<T>> querySelector) where T : class
    {
      Nest.Extensions.ThrowIfNull<Action<QueryDescriptor<T>>>(querySelector, "querySelector");
      QueryDescriptor<T> @object = new QueryDescriptor<T>();
      querySelector(@object);
      return this._Count("_count", ElasticClient.Serialize<QueryDescriptor<T>>(@object));
    }

    public CountResponse Count(Action<QueryDescriptor> querySelector)
    {
      string defaultIndex = this.Settings.DefaultIndex;
      Nest.Extensions.ThrowIfNullOrEmpty(defaultIndex, "Cannot infer default index for current connection.");
      string path = this.CreatePath(defaultIndex) + "_count";
      QueryDescriptor @object = new QueryDescriptor();
      querySelector(@object);
      string query = ElasticClient.Serialize<QueryDescriptor>(@object);
      return this._Count(path, query);
    }

    public CountResponse Count(IEnumerable<string> indices, Action<QueryDescriptor> querySelector)
    {
      Nest.Extensions.ThrowIfEmpty<string>(indices, "indices");
      string path = string.Join(",", indices) + "/_count";
      QueryDescriptor @object = new QueryDescriptor();
      querySelector(@object);
      string query = ElasticClient.Serialize<QueryDescriptor>(@object);
      return this._Count(path, query);
    }

    public CountResponse Count(IEnumerable<string> indices, IEnumerable<string> types, Action<QueryDescriptor> querySelector)
    {
      Nest.Extensions.ThrowIfEmpty<string>(indices, "indices");
      Nest.Extensions.ThrowIfEmpty<string>(indices, "types");
      string path = string.Join(",", indices) + "/" + string.Join(",", types) + "/_count";
      QueryDescriptor @object = new QueryDescriptor();
      querySelector(@object);
      string query = ElasticClient.Serialize<QueryDescriptor>(@object);
      return this._Count(path, query);
    }

    public CountResponse Count<T>(Action<QueryDescriptor<T>> querySelector) where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      string type = this.InferTypeName<T>();
      string path = this.CreatePath(indexForType, type) + "_count";
      QueryDescriptor<T> @object = new QueryDescriptor<T>();
      querySelector(@object);
      string query = ElasticClient.Serialize<QueryDescriptor<T>>(@object);
      return this._Count(path, query);
    }

    public CountResponse Count<T>(IEnumerable<string> indices, Action<QueryDescriptor<T>> querySelector) where T : class
    {
      Nest.Extensions.ThrowIfEmpty<string>(indices, "indices");
      string path = string.Join(",", indices) + "/_count";
      QueryDescriptor<T> @object = new QueryDescriptor<T>();
      querySelector(@object);
      string query = ElasticClient.Serialize<QueryDescriptor<T>>(@object);
      return this._Count(path, query);
    }

    public CountResponse Count<T>(IEnumerable<string> indices, IEnumerable<string> types, Action<QueryDescriptor<T>> querySelector) where T : class
    {
      Nest.Extensions.ThrowIfEmpty<string>(indices, "indices");
      Nest.Extensions.ThrowIfEmpty<string>(indices, "types");
      string path = string.Join(",", indices) + "/" + string.Join(",", types) + "/_count";
      QueryDescriptor<T> @object = new QueryDescriptor<T>();
      querySelector(@object);
      string query = ElasticClient.Serialize<QueryDescriptor<T>>(@object);
      return this._Count(path, query);
    }

    private CountResponse _Count(string path, string query)
    {
      return this.ToParsedResponse<CountResponse>(this.Connection.PostSync(path, query), false);
    }

    public ConnectionStatus Delete<T>(T @object) where T : class
    {
      return this._deleteToPath(this.CreatePathFor<T>(@object));
    }

    public ConnectionStatus Delete<T>(T @object, string index) where T : class
    {
      return this._deleteToPath(this.CreatePathFor<T>(@object, index));
    }

    public ConnectionStatus Delete<T>(T @object, string index, string type) where T : class
    {
      return this._deleteToPath(this.CreatePathFor<T>(@object, index, type));
    }

    public ConnectionStatus Delete<T>(T @object, DeleteParameters deleteParameters) where T : class
    {
      return this._deleteToPath(this.AppendParametersToPath(this.CreatePathFor<T>(@object), (IUrlParameters) deleteParameters));
    }

    public ConnectionStatus Delete<T>(T @object, string index, DeleteParameters deleteParameters) where T : class
    {
      return this._deleteToPath(this.AppendParametersToPath(this.CreatePathFor<T>(@object, index), (IUrlParameters) deleteParameters));
    }

    public ConnectionStatus Delete<T>(T @object, string index, string type, DeleteParameters deleteParameters) where T : class
    {
      return this._deleteToPath(this.AppendParametersToPath(this.CreatePathFor<T>(@object, index, type), (IUrlParameters) deleteParameters));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(T @object) where T : class
    {
      return this._deleteToPathAsync(this.CreatePathFor<T>(@object));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(T @object, string index) where T : class
    {
      return this._deleteToPathAsync(this.CreatePathFor<T>(@object, index));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(T @object, string index, string type) where T : class
    {
      return this._deleteToPathAsync(this.CreatePathFor<T>(@object, index, type));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(T @object, DeleteParameters deleteParameters) where T : class
    {
      return this._deleteToPathAsync(this.AppendParametersToPath(this.CreatePathFor<T>(@object), (IUrlParameters) deleteParameters));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(T @object, string index, DeleteParameters deleteParameters) where T : class
    {
      return this._deleteToPathAsync(this.AppendParametersToPath(this.CreatePathFor<T>(@object, index), (IUrlParameters) deleteParameters));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(T @object, string index, string type, DeleteParameters deleteParameters) where T : class
    {
      return this._deleteToPathAsync(this.AppendParametersToPath(this.CreatePathFor<T>(@object, index, type), (IUrlParameters) deleteParameters));
    }

    public ConnectionStatus DeleteById<T>(int id) where T : class
    {
      return this.DeleteById<T>(id.ToString());
    }

    public ConnectionStatus DeleteById<T>(string id) where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      string type = this.InferTypeName<T>();
      return this._deleteToPath(this.CreatePath(indexForType, type, id));
    }

    public ConnectionStatus DeleteById(string index, string type, string id)
    {
      return this._deleteToPath(this.CreatePath(index, type, id));
    }

    public ConnectionStatus DeleteById(string index, string type, int id)
    {
      return this._deleteToPath(this.CreatePath(index, type, id.ToString()));
    }

    public ConnectionStatus DeleteById<T>(int id, DeleteParameters deleteParameters) where T : class
    {
      return this.DeleteById<T>(id.ToString(), deleteParameters);
    }

    public ConnectionStatus DeleteById<T>(string id, DeleteParameters deleteParameters) where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      string type = this.InferTypeName<T>();
      return this._deleteToPath(this.AppendParametersToPath(this.CreatePath(indexForType, type, id), (IUrlParameters) deleteParameters));
    }

    public ConnectionStatus DeleteById(string index, string type, string id, DeleteParameters deleteParameters)
    {
      return this._deleteToPath(this.AppendParametersToPath(this.CreatePath(index, type, id), (IUrlParameters) deleteParameters));
    }

    public ConnectionStatus DeleteById(string index, string type, int id, DeleteParameters deleteParameters)
    {
      return this._deleteToPath(this.AppendParametersToPath(this.CreatePath(index, type, id.ToString()), (IUrlParameters) deleteParameters));
    }

    public Task<ConnectionStatus> DeleteByIdAsync<T>(int id) where T : class
    {
      return this.DeleteByIdAsync<T>(id.ToString());
    }

    public Task<ConnectionStatus> DeleteByIdAsync<T>(string id) where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      string type = this.InferTypeName<T>();
      return this._deleteToPathAsync(this.CreatePath(indexForType, type, id));
    }

    public Task<ConnectionStatus> DeleteByIdAsync(string index, string type, string id)
    {
      return this._deleteToPathAsync(this.CreatePath(index, type, id));
    }

    public Task<ConnectionStatus> DeleteByIdAsync(string index, string type, int id)
    {
      return this._deleteToPathAsync(this.CreatePath(index, type, id.ToString()));
    }

    public Task<ConnectionStatus> DeleteByIdAsync<T>(int id, DeleteParameters deleteParameters) where T : class
    {
      return this.DeleteByIdAsync<T>(id.ToString(), deleteParameters);
    }

    public Task<ConnectionStatus> DeleteByIdAsync<T>(string id, DeleteParameters deleteParameters) where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      string type = this.InferTypeName<T>();
      return this._deleteToPathAsync(this.AppendParametersToPath(this.CreatePath(indexForType, type, id), (IUrlParameters) deleteParameters));
    }

    public Task<ConnectionStatus> DeleteByIdAsync(string index, string type, string id, DeleteParameters deleteParameters)
    {
      return this._deleteToPathAsync(this.AppendParametersToPath(this.CreatePath(index, type, id), (IUrlParameters) deleteParameters));
    }

    public Task<ConnectionStatus> DeleteByIdAsync(string index, string type, int id, DeleteParameters deleteParameters)
    {
      return this._deleteToPathAsync(this.AppendParametersToPath(this.CreatePath(index, type, id.ToString()), (IUrlParameters) deleteParameters));
    }

    public ConnectionStatus Delete<T>(IEnumerable<T> objects) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkDeleteCommand<T>(objects));
    }

    public ConnectionStatus Delete<T>(IEnumerable<BulkParameters<T>> objects) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkDeleteCommand<T>(objects));
    }

    public ConnectionStatus Delete<T>(IEnumerable<T> objects, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public ConnectionStatus Delete<T>(IEnumerable<BulkParameters<T>> objects, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public ConnectionStatus Delete<T>(IEnumerable<T> objects, string index) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkDeleteCommand<T>(objects, index));
    }

    public ConnectionStatus Delete<T>(IEnumerable<BulkParameters<T>> objects, string index) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkDeleteCommand<T>(objects, index));
    }

    public ConnectionStatus Delete<T>(IEnumerable<T> objects, string index, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects, index);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public ConnectionStatus Delete<T>(IEnumerable<BulkParameters<T>> objects, string index, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects, index);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public ConnectionStatus Delete<T>(IEnumerable<T> objects, string index, string type) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkDeleteCommand<T>(objects, index, type));
    }

    public ConnectionStatus Delete<T>(IEnumerable<BulkParameters<T>> objects, string index, string type) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkDeleteCommand<T>(objects, index, type));
    }

    public ConnectionStatus Delete<T>(IEnumerable<T> objects, string index, string type, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects, index, type);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public ConnectionStatus Delete<T>(IEnumerable<BulkParameters<T>> objects, string index, string type, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects, index, type);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<T> objects) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkDeleteCommand<T>(objects));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<BulkParameters<T>> objects) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkDeleteCommand<T>(objects));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<T> objects, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<BulkParameters<T>> objects, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<T> objects, string index) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkDeleteCommand<T>(objects, index));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<BulkParameters<T>> objects, string index) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkDeleteCommand<T>(objects, index));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<T> objects, string index, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects, index);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<BulkParameters<T>> objects, string index, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects, index);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<T> objects, string index, string type) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkDeleteCommand<T>(objects, index, type));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<BulkParameters<T>> objects, string index, string type) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkDeleteCommand<T>(objects, index, type));
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<T> objects, string index, SimpleBulkParameters bulkParameters, string type) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects, index, type);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> DeleteAsync<T>(IEnumerable<BulkParameters<T>> objects, string index, SimpleBulkParameters bulkParameters, string type) where T : class
    {
      string data = this.GenerateBulkDeleteCommand<T>(objects, index, type);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public ConnectionStatus DeleteByQuery<T>(Action<QueryPathDescriptor<T>> query, DeleteByQueryParameters parameters = null) where T : class
    {
      QueryPathDescriptor<T> queryPathDescriptor = new QueryPathDescriptor<T>();
      query(queryPathDescriptor);
      string data = ElasticClient.Serialize<QueryPathDescriptor<T>>(queryPathDescriptor);
      string path = this.GetPathForTyped<T>(queryPathDescriptor);
      if (parameters != null)
        path = this.AppendDeleteByQueryParametersToPath(path, parameters);
      return this._deleteToPath(path, data);
    }

    public ConnectionStatus DeleteByQuery(Action<QueryPathDescriptor> query, DeleteByQueryParameters parameters = null)
    {
      QueryPathDescriptor @object = new QueryPathDescriptor();
      query(@object);
      string data = ElasticClient.Serialize<QueryPathDescriptor>(@object);
      string path = this.GetPathForDynamic((QueryPathDescriptor<object>) @object);
      if (parameters != null)
        path = this.AppendDeleteByQueryParametersToPath(path, parameters);
      return this._deleteToPath(path, data);
    }

    [Obsolete("Passing a query by string? Found a bug in the DSL? https://github.com/Mpdreamz/NEST/issues")]
    public ConnectionStatus DeleteByQuery(string query, DeleteByQueryParameters parameters = null)
    {
      string path = this.GetPathForDynamic((QueryPathDescriptor<object>) new QueryPathDescriptor());
      if (parameters != null)
        path = this.AppendDeleteByQueryParametersToPath(path, parameters);
      return this._deleteToPath(path, query);
    }

    public Task<ConnectionStatus> DeleteByQueryAsync<T>(Action<QueryPathDescriptor<T>> query, DeleteByQueryParameters parameters = null) where T : class
    {
      QueryPathDescriptor<T> queryPathDescriptor = new QueryPathDescriptor<T>();
      query(queryPathDescriptor);
      string data = ElasticClient.Serialize<QueryPathDescriptor<T>>(queryPathDescriptor);
      string path = this.GetPathForTyped<T>(queryPathDescriptor);
      if (parameters != null)
        path = this.AppendDeleteByQueryParametersToPath(path, parameters);
      return this._deleteToPathAsync(path, data);
    }

    public Task<ConnectionStatus> DeleteByQueryAsync(Action<QueryPathDescriptor> query, DeleteByQueryParameters parameters = null)
    {
      QueryPathDescriptor @object = new QueryPathDescriptor();
      query(@object);
      string data = ElasticClient.Serialize<QueryPathDescriptor>(@object);
      string path = this.GetPathForDynamic((QueryPathDescriptor<object>) @object);
      if (parameters != null)
        path = this.AppendDeleteByQueryParametersToPath(path, parameters);
      return this._deleteToPathAsync(path, data);
    }

    [Obsolete("Passing a query by string? Found a bug in the DSL? https://github.com/Mpdreamz/NEST/issues")]
    public Task<ConnectionStatus> DeleteByQueryAsync(string query, DeleteByQueryParameters parameters = null)
    {
      string path = this.GetPathForDynamic((QueryPathDescriptor<object>) new QueryPathDescriptor());
      if (parameters != null)
        path = this.AppendDeleteByQueryParametersToPath(path, parameters);
      return this._deleteToPathAsync(path, query);
    }

    private ConnectionStatus _deleteToPath(string path)
    {
      Nest.Extensions.ThrowIfNull<string>(path, "path");
      return this.Connection.DeleteSync(path);
    }

    private ConnectionStatus _deleteToPath(string path, string data)
    {
      Nest.Extensions.ThrowIfNull<string>(path, "path");
      return this.Connection.DeleteSync(path, data);
    }

    private Task<ConnectionStatus> _deleteToPathAsync(string path)
    {
      Nest.Extensions.ThrowIfNull<string>(path, "path");
      return this.Connection.Delete(path);
    }

    private Task<ConnectionStatus> _deleteToPathAsync(string path, string data)
    {
      Nest.Extensions.ThrowIfNull<string>(path, "path");
      return this.Connection.Delete(path, data);
    }

    internal string CreatePathFor<T>(T @object) where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      if (string.IsNullOrEmpty(indexForType))
        throw new NullReferenceException("Cannot infer default index for current connection.");
      else
        return this.CreatePathFor<T>(@object, indexForType);
    }

    internal string CreatePathFor<T>(T @object, string index) where T : class
    {
      string type = this.InferTypeName<T>();
      return this.CreatePathFor<T>(@object, index, type);
    }

    internal string CreatePathFor<T>(T @object, string index, string type) where T : class
    {
      Nest.Extensions.ThrowIfNull<T>(@object, "object");
      Nest.Extensions.ThrowIfNull<string>(index, "index");
      Nest.Extensions.ThrowIfNull<string>(type, "type");
      string path = this.CreatePath(index, type);
      string idFor = this.GetIdFor<T>(@object);
      if (!string.IsNullOrEmpty(idFor))
        path = this.CreatePath(index, type, idFor);
      return path;
    }

    internal string CreatePathFor<T>(T @object, string index, string type, string id) where T : class
    {
      Nest.Extensions.ThrowIfNull<T>(@object, "object");
      Nest.Extensions.ThrowIfNull<string>(index, "index");
      Nest.Extensions.ThrowIfNull<string>(type, "type");
      return this.CreatePath(index, type, id);
    }

    internal Func<T, string> CreateIdSelector<T>() where T : class
    {
      return (Func<T, string>) (@object => this.GetIdFor<T>(@object));
    }

    internal static Func<T, object> MakeDelegate<T, U>(MethodInfo get)
    {
      Func<T, U> f = (Func<T, U>) Delegate.CreateDelegate(typeof (Func<T, U>), get);
      return (Func<T, object>) (t => (object) f(t));
    }

    internal string GetIdFor<T>(T @object)
    {
      Type type = typeof (T);
      Func<object, string> func1;
      if (ElasticClient.IdDelegates.TryGetValue(type, out func1))
        return func1((object) @object);
      ElasticTypeAttribute elasticPropertyFor = ElasticClient.PropertyNameResolver.GetElasticPropertyFor(type);
      string name = elasticPropertyFor != null ? elasticPropertyFor.IdProperty : string.Empty;
      if (string.IsNullOrWhiteSpace(name))
        name = "Id";
      PropertyInfo property = type.GetProperty(name);
      if (property == (PropertyInfo) null)
        throw new Exception("Could not infer id for object of type" + type.FullName);
      try
      {
        MethodInfo getMethod = property.GetGetMethod();
        Func<T, object> func = (Func<T, object>) typeof (ElasticClient).GetMethod("MakeDelegate", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type, getMethod.ReturnType).Invoke((object) null, (object[]) new MethodInfo[1]
        {
          getMethod
        });
        Func<object, string> func2 = (Func<object, string>) (o => func((T) o).ToString());
        ElasticClient.IdDelegates.TryAdd(type, func2);
        return func2((object) @object);
      }
      catch (Exception ex)
      {
        return property.GetValue((object) @object, (object[]) null).ToString();
      }
    }

    internal static string GetTypeNameFor(Type type)
    {
      if (!type.IsClass && !type.IsInterface)
        throw new ArgumentException("Type is not a class or interface", "type");
      else
        return ElasticClient.GetTypeNameForType(type);
    }

    internal static string GetTypeNameFor<T>() where T : class
    {
      return ElasticClient.GetTypeNameForType(typeof (T));
    }

    internal static string GetTypeNameForType(Type type)
    {
      string name = type.Name;
      ElasticTypeAttribute elasticPropertyFor = ElasticClient.PropertyNameResolver.GetElasticPropertyFor(type);
      return elasticPropertyFor == null || Nest.Extensions.IsNullOrEmpty(elasticPropertyFor.Name) ? Inflector.MakePlural(type.Name).ToLower() : elasticPropertyFor.Name;
    }

    internal string InferTypeName<T>() where T : class
    {
      return this.InferTypeName(typeof (T));
    }

    internal string InferTypeName(Type type)
    {
      string str = type.Name;
      ElasticTypeAttribute elasticPropertyFor = ElasticClient.PropertyNameResolver.GetElasticPropertyFor(type);
      if (elasticPropertyFor != null && !Nest.Extensions.IsNullOrEmpty(elasticPropertyFor.Name))
      {
        str = elasticPropertyFor.Name;
      }
      else
      {
        if (this.Settings.TypeNameInferrer != null)
          str = this.Settings.TypeNameInferrer(str);
        if (this.Settings.TypeNameInferrer == null || string.IsNullOrEmpty(str))
          str = Inflector.MakePlural(type.Name).ToLower();
      }
      return str;
    }

    internal string CreatePath(string index)
    {
      Nest.Extensions.ThrowIfNullOrEmpty(index, "index");
      return Nest.Extensions.F("{0}/", new object[1]
      {
        (object) Uri.EscapeDataString(index)
      });
    }

    internal string CreatePath(string index, string type)
    {
      Nest.Extensions.ThrowIfNullOrEmpty(index, "index");
      Nest.Extensions.ThrowIfNullOrEmpty(type, "type");
      return Nest.Extensions.F("{0}/{1}/", (object) Uri.EscapeDataString(index), (object) Uri.EscapeDataString(type));
    }

    internal string CreatePath(string index, string type, string id)
    {
      Nest.Extensions.ThrowIfNullOrEmpty(index, "index");
      Nest.Extensions.ThrowIfNullOrEmpty(type, "type");
      Nest.Extensions.ThrowIfNullOrEmpty(id, "id");
      return Nest.Extensions.F("{0}/{1}/{2}", (object) Uri.EscapeDataString(index), (object) Uri.EscapeDataString(type), (object) Uri.EscapeDataString(id));
    }

    internal string GenerateBulkIndexCommand<T>(IEnumerable<T> objects) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, "index");
    }

    private string GenerateBulkIndexCommand<T>(IEnumerable<BulkParameters<T>> objects) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, "index");
    }

    private string GenerateBulkIndexCommand<T>(IEnumerable<T> objects, string index) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, index, "index");
    }

    private string GenerateBulkIndexCommand<T>(IEnumerable<BulkParameters<T>> objects, string index) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, index, "index");
    }

    private string GenerateBulkIndexCommand<T>(IEnumerable<T> objects, string index, string typeName) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, index, typeName, "index");
    }

    private string GenerateBulkIndexCommand<T>(IEnumerable<BulkParameters<T>> objects, string index, string typeName) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, index, typeName, "index");
    }

    private string GenerateBulkDeleteCommand<T>(IEnumerable<T> objects) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, "delete");
    }

    private string GenerateBulkDeleteCommand<T>(IEnumerable<BulkParameters<T>> objects) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, "delete");
    }

    private string GenerateBulkDeleteCommand<T>(IEnumerable<T> objects, string index) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, index, "delete");
    }

    private string GenerateBulkDeleteCommand<T>(IEnumerable<BulkParameters<T>> objects, string index) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, index, "delete");
    }

    private string GenerateBulkDeleteCommand<T>(IEnumerable<T> objects, string index, string typeName) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, index, typeName, "delete");
    }

    private string GenerateBulkDeleteCommand<T>(IEnumerable<BulkParameters<T>> objects, string index, string typeName) where T : class
    {
      return this.GenerateBulkCommand<T>(objects, index, typeName, "delete");
    }

    private string GenerateBulkCommand<T>(IEnumerable<T> objects, string command) where T : class
    {
      Nest.Extensions.ThrowIfNull<IEnumerable<T>>(objects, "objects");
      string indexForType = this.Settings.GetIndexForType<T>();
      if (string.IsNullOrEmpty(indexForType))
        throw new NullReferenceException("Cannot infer default index for current connection.");
      else
        return this.GenerateBulkCommand<T>(objects, indexForType, command);
    }

    private string GenerateBulkCommand<T>(IEnumerable<BulkParameters<T>> objects, string command) where T : class
    {
      Nest.Extensions.ThrowIfNull<IEnumerable<BulkParameters<T>>>(objects, "objects");
      string indexForType = this.Settings.GetIndexForType<T>();
      if (string.IsNullOrEmpty(indexForType))
        throw new NullReferenceException("Cannot infer default index for current connection.");
      else
        return this.GenerateBulkCommand<T>(objects, indexForType, command);
    }

    private string GenerateBulkCommand<T>(IEnumerable<T> objects, string index, string command) where T : class
    {
      Nest.Extensions.ThrowIfNull<IEnumerable<T>>(objects, "objects");
      Nest.Extensions.ThrowIfNullOrEmpty(index, "index");
      string typeName = this.InferTypeName<T>();
      return this.GenerateBulkCommand<T>(objects, index, typeName, command);
    }

    private string GenerateBulkCommand<T>(IEnumerable<BulkParameters<T>> objects, string index, string command) where T : class
    {
      Nest.Extensions.ThrowIfNull<IEnumerable<BulkParameters<T>>>(objects, "objects");
      Nest.Extensions.ThrowIfNullOrEmpty(index, "index");
      string typeName = this.InferTypeName<T>();
      return this.GenerateBulkCommand<T>(objects, index, typeName, command);
    }

    private string GenerateBulkCommand<T>(IEnumerable<T> objects, string index, string typeName, string command) where T : class
    {
      if (!Enumerable.Any<T>(objects))
        return (string) null;
      Func<T, string> idSelector = this.CreateIdSelector<T>();
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = Nest.Extensions.F("{{ \"{0}\" : {{ \"_index\" : \"{1}\", \"_type\" : \"{2}\"", (object) command, (object) index, (object) typeName);
      foreach (T obj in objects)
      {
        string str2 = str1;
        if (idSelector != null)
          str2 = str2 + Nest.Extensions.F(", \"_id\" : \"{0}\" ", new object[1]
          {
            (object) idSelector(obj)
          });
        string str3 = str2 + "} }\n";
        stringBuilder.Append(str3);
        if (command == "index")
        {
          string str4 = JsonConvert.SerializeObject((object) obj, Formatting.None, ElasticClient.SerializationSettings);
          stringBuilder.Append(str4 + "\n");
        }
      }
      return ((object) stringBuilder).ToString();
    }

    private string GenerateBulkCommand<T>(IEnumerable<BulkParameters<T>> objects, string index, string typeName, string command) where T : class
    {
      if (!Enumerable.Any<BulkParameters<T>>(objects))
        return (string) null;
      Func<T, string> idSelector = this.CreateIdSelector<T>();
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = Nest.Extensions.F("{{ \"{0}\" : {{ \"_index\" : \"{1}\", \"_type\" : \"{2}\"", (object) command, (object) index, (object) typeName);
      foreach (BulkParameters<T> bulkParameters in objects)
      {
        if ((object) bulkParameters.Document != null)
        {
          string str2 = str1;
          if (idSelector != null)
            str2 = str2 + Nest.Extensions.F(", \"_id\" : \"{0}\" ", new object[1]
            {
              (object) idSelector(bulkParameters.Document)
            });
          if (!Nest.Extensions.IsNullOrEmpty(bulkParameters.Version))
            str2 = str2 + Nest.Extensions.F(", \"version\" : \"{0}\" ", new object[1]
            {
              (object) bulkParameters.Version
            });
          if (!Nest.Extensions.IsNullOrEmpty(bulkParameters.Parent))
            str2 = str2 + Nest.Extensions.F(", \"parent\" : \"{0}\" ", new object[1]
            {
              (object) bulkParameters.Parent
            });
          if (bulkParameters.VersionType != VersionType.Internal)
            str2 = str2 + Nest.Extensions.F(", \"version_type\" : \"{0}\" ", new object[1]
            {
              (object) ((object) bulkParameters.VersionType).ToString().ToLower()
            });
          if (!Nest.Extensions.IsNullOrEmpty(bulkParameters.Routing))
            str2 = str2 + Nest.Extensions.F(", \"routing\" : \"{0}\" ", new object[1]
            {
              (object) bulkParameters.Routing
            });
          string str3 = str2 + "} }\n";
          stringBuilder.Append(str3);
          if (command == "index")
          {
            string str4 = JsonConvert.SerializeObject((object) bulkParameters.Document, Formatting.None, ElasticClient.SerializationSettings);
            stringBuilder.Append(str4 + "\n");
          }
        }
      }
      return ((object) stringBuilder).ToString();
    }

    private string AppendSimpleParametersToPath(string path, ISimpleUrlParameters urlParameters)
    {
      if (urlParameters == null)
        return path;
      List<string> list = new List<string>();
      if (urlParameters.Replication != Replication.Sync)
        list.Add("replication=" + ((object) urlParameters.Replication).ToString().ToLower());
      if (urlParameters.Refresh)
        list.Add("refresh=true");
      path = path + "?" + string.Join("&", list.ToArray());
      return path;
    }

    private string AppendDeleteByQueryParametersToPath(string path, DeleteByQueryParameters urlParameters)
    {
      if (urlParameters == null)
        return path;
      List<string> list = new List<string>();
      if (urlParameters.Replication != Replication.Sync)
        list.Add("replication=" + ((object) urlParameters.Replication).ToString().ToLower());
      if (urlParameters.Consistency != Consistency.Quorum)
        list.Add("consistency=" + ((object) urlParameters.Replication).ToString().ToLower());
      if (!Nest.Extensions.IsNullOrEmpty(urlParameters.Routing))
        list.Add("routing=" + urlParameters.Routing);
      path = path + "?" + string.Join("&", list.ToArray());
      return path;
    }

    private string AppendParametersToPath(string path, IUrlParameters urlParameters)
    {
      if (urlParameters == null)
        return path;
      List<string> parameters = new List<string>();
      if (!Nest.Extensions.IsNullOrEmpty(urlParameters.Version))
        parameters.Add("version=" + urlParameters.Version);
      if (!Nest.Extensions.IsNullOrEmpty(urlParameters.Routing))
        parameters.Add("routing=" + urlParameters.Routing);
      if (!Nest.Extensions.IsNullOrEmpty(urlParameters.Parent))
        parameters.Add("parent=" + urlParameters.Parent);
      if (urlParameters.Replication != Replication.Sync)
        parameters.Add("replication=" + ((object) urlParameters.Replication).ToString().ToLower());
      if (urlParameters.Consistency != Consistency.Quorum)
        parameters.Add("consistency=" + ((object) urlParameters.Consistency).ToString().ToLower());
      if (urlParameters.Refresh)
        parameters.Add("refresh=true");
      if (urlParameters is IndexParameters)
        this.AppendIndexParameters(parameters, (IndexParameters) urlParameters);
      path = path + "?" + string.Join("&", parameters.ToArray());
      return path;
    }

    private List<string> AppendIndexParameters(List<string> parameters, IndexParameters indexParameters)
    {
      if (indexParameters == null)
        return parameters;
      if (!Nest.Extensions.IsNullOrEmpty(indexParameters.Timeout))
        parameters.Add("timeout=" + indexParameters.Timeout);
      if (indexParameters.VersionType != VersionType.Internal)
        parameters.Add("version_type=" + ((object) indexParameters.VersionType).ToString().ToLower());
      return parameters;
    }

    private string GetPathForDynamic(SearchDescriptor<object> descriptor)
    {
      string indices = this.Settings.DefaultIndex;
      if (Nest.Extensions.HasAny<string>(descriptor._Indices))
        indices = string.Join(",", descriptor._Indices);
      else if (descriptor._Indices != null || descriptor._AllIndices)
        indices = "_all";
      string types = Nest.Extensions.HasAny<string>(descriptor._Types) ? string.Join(",", descriptor._Types) : (string) null;
      return this.PathJoin(indices, types, descriptor._Routing, "_search");
    }

    private string GetPathForTyped<T>(SearchDescriptor<T> descriptor) where T : class
    {
      string indices = this.Settings.GetIndexForType<T>();
      if (Nest.Extensions.HasAny<string>(descriptor._Indices))
        indices = string.Join(",", descriptor._Indices);
      else if (descriptor._Indices != null || descriptor._AllIndices)
        indices = "_all";
      string types = this.InferTypeName<T>();
      if (Nest.Extensions.HasAny<string>(descriptor._Types))
        types = string.Join(",", descriptor._Types);
      else if (descriptor._Types != null || descriptor._AllTypes)
        types = (string) null;
      return this.PathJoin(indices, types, descriptor._Routing, "_search");
    }

    private string GetPathForDynamic(QueryPathDescriptor<object> descriptor)
    {
      string indices = this.Settings.DefaultIndex;
      if (Nest.Extensions.HasAny<string>(descriptor._Indices))
        indices = string.Join(",", descriptor._Indices);
      else if (descriptor._Indices != null || descriptor._AllIndices)
        indices = "_all";
      string types = Nest.Extensions.HasAny<string>(descriptor._Types) ? string.Join(",", descriptor._Types) : (string) null;
      return this.PathJoin(indices, types, descriptor._Routing, "_query");
    }

    private string GetPathForTyped<T>(QueryPathDescriptor<T> descriptor) where T : class
    {
      string indices = this.Settings.GetIndexForType<T>();
      if (Nest.Extensions.HasAny<string>(descriptor._Indices))
        indices = string.Join(",", descriptor._Indices);
      else if (descriptor._Indices != null || descriptor._AllIndices)
        indices = "_all";
      string types = this.InferTypeName<T>();
      if (Nest.Extensions.HasAny<string>(descriptor._Types))
        types = string.Join(",", descriptor._Types);
      else if (descriptor._Types != null || descriptor._AllTypes)
        types = (string) null;
      return this.PathJoin(indices, types, descriptor._Routing, "_query");
    }

    private string PathJoin(string indices, string types, string routing, string extension = "_search")
    {
      string str = (!string.IsNullOrEmpty(types) ? this.CreatePath(indices, types) : this.CreatePath(indices)) + extension;
      if (!string.IsNullOrEmpty(routing))
        str = Nest.Extensions.F("{0}?routing={1}", (object) str, (object) routing);
      return str;
    }

    public IndicesResponse DeleteMapping<T>() where T : class
    {
      return this.DeleteMapping<T>(this.Settings.GetIndexForType<T>(), this.InferTypeName<T>());
    }

    public IndicesResponse DeleteMapping<T>(string index) where T : class
    {
      string type = this.InferTypeName<T>();
      return this.DeleteMapping<T>(index, type);
    }

    public IndicesResponse DeleteMapping<T>(string index, string type) where T : class
    {
      ConnectionStatus connectionStatus = this.Connection.DeleteSync(this.CreatePath(index, type));
      IndicesResponse indicesResponse = new IndicesResponse();
      try
      {
        indicesResponse = JsonConvert.DeserializeObject<IndicesResponse>(connectionStatus.Result);
      }
      catch
      {
      }
      indicesResponse.ConnectionStatus = connectionStatus;
      return indicesResponse;
    }

    public IndicesResponse DeleteMapping(Type t)
    {
      string indexForType = this.Settings.GetIndexForType(t);
      string type = this.InferTypeName(t);
      return this.DeleteMapping(t, indexForType, type);
    }

    public IndicesResponse DeleteMapping(Type t, string index)
    {
      string type = this.InferTypeName(t);
      return this.DeleteMapping(t, index, type);
    }

    public IndicesResponse DeleteMapping(Type t, string index, string type)
    {
      ConnectionStatus connectionStatus = this.Connection.DeleteSync(this.CreatePath(index, type));
      IndicesResponse indicesResponse = new IndicesResponse();
      try
      {
        indicesResponse = JsonConvert.DeserializeObject<IndicesResponse>(connectionStatus.Result);
      }
      catch
      {
      }
      indicesResponse.ConnectionStatus = connectionStatus;
      return indicesResponse;
    }

    public IndicesResponse Map<T>() where T : class
    {
      return this.Map<T>(this.Settings.GetIndexForType<T>(), this.InferTypeName<T>());
    }

    public IndicesResponse Map<T>(string index) where T : class
    {
      string type = this.InferTypeName<T>();
      return this.Map<T>(index, type);
    }

    public IndicesResponse Map<T>(string index, string type) where T : class
    {
      ConnectionStatus connectionStatus = this.Connection.PutSync(this.CreatePath(index, type) + "_mapping", this.CreateMapFor<T>(type));
      IndicesResponse indicesResponse = new IndicesResponse();
      try
      {
        indicesResponse = JsonConvert.DeserializeObject<IndicesResponse>(connectionStatus.Result);
        indicesResponse.IsValid = true;
      }
      catch
      {
      }
      indicesResponse.ConnectionStatus = connectionStatus;
      return indicesResponse;
    }

    public IndicesResponse Map(Type t)
    {
      string type = this.InferTypeName(t);
      return this.Map(t, this.Settings.GetIndexForType(t), type);
    }

    public IndicesResponse Map(Type t, string index)
    {
      string type = this.InferTypeName(t);
      return this.Map(t, index, type);
    }

    public IndicesResponse Map(Type t, string index, string type)
    {
      ConnectionStatus connectionStatus = this.Connection.PutSync(this.CreatePath(index, type) + "_mapping", this.CreateMapFor(t, type));
      IndicesResponse indicesResponse = new IndicesResponse();
      try
      {
        indicesResponse = JsonConvert.DeserializeObject<IndicesResponse>(connectionStatus.Result);
        indicesResponse.IsValid = true;
      }
      catch
      {
      }
      indicesResponse.ConnectionStatus = connectionStatus;
      return indicesResponse;
    }

    public IndicesResponse Map(TypeMapping typeMapping)
    {
      return this.Map(typeMapping, this.Settings.DefaultIndex);
    }

    public IndicesResponse Map(TypeMapping typeMapping, string index)
    {
      return this.ToParsedResponse<IndicesResponse>(this.Connection.PutSync(this.CreatePath(index, typeMapping.Name) + "_mapping", JsonConvert.SerializeObject((object) new Dictionary<string, TypeMapping>()
      {
        {
          typeMapping.Name,
          typeMapping
        }
      }, Formatting.None, ElasticClient.SerializationSettings)), false);
    }

    public TypeMapping GetMapping<T>() where T : class
    {
      this.Settings.GetIndexForType<T>();
      return this.GetMapping<T>();
    }

    public TypeMapping GetMapping<T>(string index) where T : class
    {
      string type = this.InferTypeName<T>();
      return this.GetMapping(index, type);
    }

    public TypeMapping GetMapping(Type t)
    {
      string indexForType = this.Settings.GetIndexForType(t);
      return this.GetMapping(t, indexForType);
    }

    public TypeMapping GetMapping(Type t, string index)
    {
      string type = this.InferTypeName(t);
      return this.GetMapping(index, type);
    }

    public TypeMapping GetMapping(string index, string type)
    {
      ConnectionStatus sync = this.Connection.GetSync(this.CreatePath(index, type) + "_mapping");
      try
      {
        IDictionary<string, TypeMapping> dictionary = JsonConvert.DeserializeObject<IDictionary<string, TypeMapping>>(sync.Result, ElasticClient.SerializationSettings);
        if (sync.Success)
        {
          KeyValuePair<string, TypeMapping> keyValuePair = Enumerable.First<KeyValuePair<string, TypeMapping>>((IEnumerable<KeyValuePair<string, TypeMapping>>) dictionary);
          keyValuePair.Value.Name = keyValuePair.Key;
          return keyValuePair.Value;
        }
      }
      catch (Exception ex)
      {
      }
      return (TypeMapping) null;
    }

    private string CreateMapFor<T>(string type) where T : class
    {
      return this.CreateMapFor(typeof (T), type);
    }

    private string CreateMapFor(Type t, string type)
    {
      return new TypeMappingWriter(t, type, ElasticClient.PropertyNameResolver).MapFromAttributes();
    }

    public T Get<T>(int id) where T : class
    {
      return this.Get<T>(id.ToString());
    }

    public T Get<T>(string id) where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      string type = this.InferTypeName<T>();
      return this.Get<T>(id, this.CreatePath(indexForType, type));
    }

    public T Get<T>(string index, string type, string id) where T : class
    {
      return this.Get<T>(id, index + "/" + type + "/");
    }

    public T Get<T>(string index, string type, int id) where T : class
    {
      return this.Get<T>(id.ToString(), index + "/" + type + "/");
    }

    private T Get<T>(string id, string path) where T : class
    {
      ConnectionStatus sync = this.Connection.GetSync(path + id);
      if (sync.Result == null)
        return default (T);
      JToken jtoken = JObject.Parse(sync.Result)["_source"];
      if (jtoken != null)
        return JsonConvert.DeserializeObject<T>(((object) jtoken).ToString());
      else
        return default (T);
    }

    public IEnumerable<T> Get<T>(IEnumerable<int> ids) where T : class
    {
      return this.Get<T>(Enumerable.Select<int, string>(ids, (Func<int, string>) (i => Convert.ToString(i))));
    }

    public IEnumerable<T> Get<T>(IEnumerable<string> ids) where T : class
    {
      string indexForType = this.Settings.GetIndexForType<T>();
      Nest.Extensions.ThrowIfNullOrEmpty(indexForType, "Cannot infer default index for current connection.");
      string type = this.InferTypeName<T>();
      return this.Get<T>(ids, this.CreatePath(indexForType, type));
    }

    public IEnumerable<T> Get<T>(string index, string type, IEnumerable<int> ids) where T : class
    {
      return this.Get<T>(index, type, Enumerable.Select<int, string>(ids, (Func<int, string>) (i => Convert.ToString(i))));
    }

    public IEnumerable<T> Get<T>(string index, string type, IEnumerable<string> ids) where T : class
    {
      return this.Get<T>(ids, this.CreatePath(index, type));
    }

    private IEnumerable<T> Get<T>(IEnumerable<string> ids, string path) where T : class
    {
      string data = Nest.Extensions.F("{{ \"ids\": {0} }}", new object[1]
      {
        (object) JsonConvert.SerializeObject((object) ids)
      });
      ConnectionStatus connectionStatus = this.Connection.PostSync(path + "_mget", data);
      if (connectionStatus.Result == null)
        return (IEnumerable<T>) null;
      else
        return Enumerable.Select<Hit<T>, T>(Enumerable.Where<Hit<T>>(JsonConvert.DeserializeObject<MultiHit<T>>(connectionStatus.Result).Hits, (Func<Hit<T>, bool>) (h => (object) h.Source != null)), (Func<Hit<T>, T>) (h => h.Source));
    }

    public ConnectionStatus Index<T>(T @object) where T : class
    {
      string pathFor = this.CreatePathFor<T>(@object);
      return this._indexToPath<T>(@object, pathFor);
    }

    public ConnectionStatus Index<T>(T @object, IndexParameters indexParameters) where T : class
    {
      string path = this.AppendParametersToPath(this.CreatePathFor<T>(@object), (IUrlParameters) indexParameters);
      return this._indexToPath<T>(@object, path);
    }

    public ConnectionStatus Index<T>(T @object, string index) where T : class
    {
      string pathFor = this.CreatePathFor<T>(@object, index);
      return this._indexToPath<T>(@object, pathFor);
    }

    public ConnectionStatus Index<T>(T @object, string index, IndexParameters indexParameters) where T : class
    {
      string path = this.AppendParametersToPath(this.CreatePathFor<T>(@object, index), (IUrlParameters) indexParameters);
      return this._indexToPath<T>(@object, path);
    }

    public ConnectionStatus Index<T>(T @object, string index, string type) where T : class
    {
      string pathFor = this.CreatePathFor<T>(@object, index, type);
      return this._indexToPath<T>(@object, pathFor);
    }

    public ConnectionStatus Index<T>(T @object, string index = null, string type = null, IndexParameters indexParameters = null) where T : class
    {
      string path = this.AppendParametersToPath(this.CreatePathFor<T>(@object, index, type), (IUrlParameters) indexParameters);
      return this._indexToPath<T>(@object, path);
    }

    public ConnectionStatus Index<T>(T @object, string index, string type, string id) where T : class
    {
      string pathFor = this.CreatePathFor<T>(@object, index, type, id);
      return this._indexToPath<T>(@object, pathFor);
    }

    public ConnectionStatus Index<T>(T @object, string index, string type, string id, IndexParameters indexParameters) where T : class
    {
      string path = this.AppendParametersToPath(this.CreatePathFor<T>(@object, index, type, id), (IUrlParameters) indexParameters);
      return this._indexToPath<T>(@object, path);
    }

    public ConnectionStatus Index<T>(T @object, string index, string type, int id) where T : class
    {
      string pathFor = this.CreatePathFor<T>(@object, index, type, id.ToString());
      return this._indexToPath<T>(@object, pathFor);
    }

    public ConnectionStatus Index<T>(T @object, string index, string type, int id, IndexParameters indexParameters) where T : class
    {
      string path = this.AppendParametersToPath(this.CreatePathFor<T>(@object, index, type, id.ToString()), (IUrlParameters) indexParameters);
      return this._indexToPath<T>(@object, path);
    }

    private ConnectionStatus _indexToPath<T>(T @object, string path) where T : class
    {
      Nest.Extensions.ThrowIfNull<string>(path, "path");
      string data = JsonConvert.SerializeObject((object) @object, Formatting.Indented, ElasticClient.SerializationSettings);
      return this.Connection.PostSync(path, data);
    }

    public Task<ConnectionStatus> IndexAsync<T>(T @object) where T : class
    {
      string pathFor = this.CreatePathFor<T>(@object);
      return this._indexAsyncToPath<T>(@object, pathFor);
    }

    public Task<ConnectionStatus> IndexAsync<T>(T @object, IndexParameters indexParameters) where T : class
    {
      string path = this.AppendParametersToPath(this.CreatePathFor<T>(@object), (IUrlParameters) indexParameters);
      return this._indexAsyncToPath<T>(@object, path);
    }

    public Task<ConnectionStatus> IndexAsync<T>(T @object, string index) where T : class
    {
      string pathFor = this.CreatePathFor<T>(@object, index);
      return this._indexAsyncToPath<T>(@object, pathFor);
    }

    public Task<ConnectionStatus> IndexAsync<T>(T @object, string index, IndexParameters indexParameters) where T : class
    {
      string path = this.AppendParametersToPath(this.CreatePathFor<T>(@object, index), (IUrlParameters) indexParameters);
      return this._indexAsyncToPath<T>(@object, path);
    }

    public Task<ConnectionStatus> IndexAsync<T>(T @object, string index, string type) where T : class
    {
      string pathFor = this.CreatePathFor<T>(@object, index, type);
      return this._indexAsyncToPath<T>(@object, pathFor);
    }

    public Task<ConnectionStatus> IndexAsync<T>(T @object, string index, string type, IndexParameters indexParameters) where T : class
    {
      string path = this.AppendParametersToPath(this.CreatePathFor<T>(@object, index, type), (IUrlParameters) indexParameters);
      return this._indexAsyncToPath<T>(@object, path);
    }

    public Task<ConnectionStatus> IndexAsync<T>(T @object, string index, string type, string id) where T : class
    {
      string pathFor = this.CreatePathFor<T>(@object, index, type, id);
      return this._indexAsyncToPath<T>(@object, pathFor);
    }

    public Task<ConnectionStatus> IndexAsync<T>(T @object, string index, string type, string id, IndexParameters indexParameters) where T : class
    {
      string path = this.AppendParametersToPath(this.CreatePathFor<T>(@object, index, type, id), (IUrlParameters) indexParameters);
      return this._indexAsyncToPath<T>(@object, path);
    }

    public Task<ConnectionStatus> IndexAsync<T>(T @object, string index, string type, int id) where T : class
    {
      string pathFor = this.CreatePathFor<T>(@object, index, type, id.ToString());
      return this._indexAsyncToPath<T>(@object, pathFor);
    }

    public Task<ConnectionStatus> IndexAsync<T>(T @object, string index, string type, int id, IndexParameters indexParameters) where T : class
    {
      string path = this.AppendParametersToPath(this.CreatePathFor<T>(@object, index, type, id.ToString()), (IUrlParameters) indexParameters);
      return this._indexAsyncToPath<T>(@object, path);
    }

    private Task<ConnectionStatus> _indexAsyncToPath<T>(T @object, string path) where T : class
    {
      string data = JsonConvert.SerializeObject((object) @object, Formatting.None, ElasticClient.SerializationSettings);
      return this.Connection.Post(path, data);
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<T> objects) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkIndexCommand<T>(objects));
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<BulkParameters<T>> objects) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkIndexCommand<T>(objects));
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<T> objects, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<BulkParameters<T>> objects, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<T> objects, string index) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkIndexCommand<T>(objects, index));
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<BulkParameters<T>> objects, string index) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkIndexCommand<T>(objects, index));
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<T> objects, string index, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects, index);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<BulkParameters<T>> objects, string index, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects, index);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<T> objects, string index, string type) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkIndexCommand<T>(objects, index, type));
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<BulkParameters<T>> objects, string index, string type) where T : class
    {
      return this.Connection.PostSync("_bulk", this.GenerateBulkIndexCommand<T>(objects, index, type));
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<T> objects, string index, string type, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects, index, type);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public ConnectionStatus IndexMany<T>(IEnumerable<BulkParameters<T>> objects, string index, string type, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects, index, type);
      return this.Connection.PostSync(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<T> objects) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkIndexCommand<T>(objects));
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<BulkParameters<T>> objects) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkIndexCommand<T>(objects));
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<T> objects, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<BulkParameters<T>> objects, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<T> objects, string index) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkIndexCommand<T>(objects, index));
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<BulkParameters<T>> objects, string index) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkIndexCommand<T>(objects, index));
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<T> objects, string index, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects, index);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<BulkParameters<T>> objects, string index, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects, index);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<T> objects, string index, string type) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkIndexCommand<T>(objects, index, type));
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<BulkParameters<T>> objects, string index, string type) where T : class
    {
      return this.Connection.Post("_bulk", this.GenerateBulkIndexCommand<T>(objects, index, type));
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<T> objects, string index, string type, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects, index, type);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public Task<ConnectionStatus> IndexManyAsync<T>(IEnumerable<BulkParameters<T>> objects, string index, string type, SimpleBulkParameters bulkParameters) where T : class
    {
      string data = this.GenerateBulkIndexCommand<T>(objects, index, type);
      return this.Connection.Post(this.AppendSimpleParametersToPath("_bulk", (ISimpleUrlParameters) bulkParameters), data);
    }

    public bool TryConnect(out ConnectionStatus status)
    {
      try
      {
        status = this.GetNodeInfo();
        return this.IsValid;
      }
      catch (Exception ex)
      {
        status = new ConnectionStatus(ex);
      }
      return false;
    }

    public string Serialize(object @object)
    {
      return JsonConvert.SerializeObject(@object, Formatting.Indented, ElasticClient.SerializationSettings);
    }

    private R ToResponse<R>(ConnectionStatus status, bool allow404 = false) where R : BaseResponse
    {
      bool flag = allow404 ? status.Error == null || status.Error.HttpStatusCode == HttpStatusCode.NotFound : status.Error == null;
      R r = (R) Activator.CreateInstance(typeof (R));
      r.IsValid = flag;
      r.ConnectionStatus = status;
      r.PropertyNameResolver = ElasticClient.PropertyNameResolver;
      return r;
    }

    private R ToParsedResponse<R>(ConnectionStatus status, bool allow404 = false) where R : BaseResponse
    {
      bool flag = allow404 ? status.Error == null || status.Error.HttpStatusCode == HttpStatusCode.NotFound : status.Error == null;
      if (!flag)
        return this.ToResponse<R>(status, allow404);
      R r = JsonConvert.DeserializeObject<R>(status.Result, ElasticClient.DeserializeSettings);
      r.IsValid = flag;
      r.ConnectionStatus = status;
      r.PropertyNameResolver = ElasticClient.PropertyNameResolver;
      return r;
    }

    private ConnectionStatus GetNodeInfo()
    {
      try
      {
        ConnectionStatus sync = this.Connection.GetSync("");
        if (sync.Success)
        {
          JObject jobject = JObject.Parse(sync.Result);
          if (jobject["ok"] == null)
          {
            this._IsValid = false;
            return sync;
          }
          else
          {
            this._IsValid = (bool) jobject["ok"];
            this._VersionInfo = JsonConvert.DeserializeObject<ElasticSearchVersionInfo>(((object) (jobject["version"] as JObject)).ToString(), ElasticClient.DeserializeSettings);
            this._gotNodeInfo = true;
          }
        }
        return sync;
      }
      catch (Exception ex)
      {
        this._IsValid = false;
        return new ConnectionStatus(ex);
      }
    }

    public QueryResponse<object> Search(Func<SearchDescriptor<object>, SearchDescriptor<object>> searcher)
    {
      SearchDescriptor<object> searchDescriptor1 = new SearchDescriptor<object>();
      SearchDescriptor<object> searchDescriptor2 = searcher(searchDescriptor1);
      return this.ToParsedResponse<QueryResponse<object>>(this.Connection.PostSync(this.GetPathForDynamic(searchDescriptor2), ElasticClient.Serialize<SearchDescriptor<object>>(searchDescriptor2)), false);
    }

    public QueryResponse<T> Search<T>(Func<SearchDescriptor<T>, SearchDescriptor<T>> searcher) where T : class
    {
      SearchDescriptor<T> searchDescriptor1 = new SearchDescriptor<T>();
      SearchDescriptor<T> searchDescriptor2 = searcher(searchDescriptor1);
      string data = ElasticClient.Serialize<SearchDescriptor<T>>(searchDescriptor2);
      return this.ToParsedResponse<QueryResponse<T>>(this.Connection.PostSync(this.GetPathForTyped<T>(searchDescriptor2), data), false);
    }

    [Obsolete("Passing a query by string? Found a bug in the DSL? https://github.com/Mpdreamz/NEST/issues")]
    public QueryResponse<T> Search<T>(string query) where T : class
    {
      return this.ToParsedResponse<QueryResponse<T>>(this.Connection.PostSync(this.GetPathForTyped<T>(new SearchDescriptor<T>()), query), false);
    }
  }
}
