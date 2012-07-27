// Type: Newtonsoft.Json.JsonSerializerSettings
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// Assembly location: E:\Dev\git\HTA\src\HTA\packages\Newtonsoft.Json.4.5.7\lib\net40\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies the settings on a <see cref="T:Newtonsoft.Json.JsonSerializer"/> object.
  /// 
  /// </summary>
  public class JsonSerializerSettings
  {
    internal static readonly StreamingContext DefaultContext = new StreamingContext();
    internal static readonly CultureInfo DefaultCulture = CultureInfo.InvariantCulture;
    internal const ReferenceLoopHandling DefaultReferenceLoopHandling = 0;
    internal const MissingMemberHandling DefaultMissingMemberHandling = 0;
    internal const NullValueHandling DefaultNullValueHandling = 0;
    internal const DefaultValueHandling DefaultDefaultValueHandling = 0;
    internal const ObjectCreationHandling DefaultObjectCreationHandling = 0;
    internal const PreserveReferencesHandling DefaultPreserveReferencesHandling = 0;
    internal const ConstructorHandling DefaultConstructorHandling = 0;
    internal const TypeNameHandling DefaultTypeNameHandling = 0;
    internal const FormatterAssemblyStyle DefaultTypeNameAssemblyFormat = 0;
    internal const Formatting DefaultFormatting = 0;
    internal const DateFormatHandling DefaultDateFormatHandling = 0;
    internal const DateTimeZoneHandling DefaultDateTimeZoneHandling = 3;
    internal const DateParseHandling DefaultDateParseHandling = 1;
    internal const bool DefaultCheckAdditionalContent = false;
    internal Formatting? _formatting;
    internal DateFormatHandling? _dateFormatHandling;
    internal DateTimeZoneHandling? _dateTimeZoneHandling;
    internal DateParseHandling? _dateParseHandling;
    internal CultureInfo _culture;
    internal bool? _checkAdditionalContent;
    internal int? _maxDepth;
    internal bool _maxDepthSet;

    /// <summary>
    /// Gets or sets how reference loops (e.g. a class referencing itself) is handled.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// Reference loop handling.
    /// </value>
    public ReferenceLoopHandling ReferenceLoopHandling { get; set; }

    /// <summary>
    /// Gets or sets how missing members (e.g. JSON contains a property that isn't a member on the object) are handled during deserialization.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// Missing member handling.
    /// </value>
    public MissingMemberHandling MissingMemberHandling { get; set; }

    /// <summary>
    /// Gets or sets how objects are created during deserialization.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The object creation handling.
    /// </value>
    public ObjectCreationHandling ObjectCreationHandling { get; set; }

    /// <summary>
    /// Gets or sets how null values are handled during serialization and deserialization.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// Null value handling.
    /// </value>
    public NullValueHandling NullValueHandling { get; set; }

    /// <summary>
    /// Gets or sets how null default are handled during serialization and deserialization.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The default value handling.
    /// </value>
    public DefaultValueHandling DefaultValueHandling { get; set; }

    /// <summary>
    /// Gets or sets a collection <see cref="T:Newtonsoft.Json.JsonConverter"/> that will be used during serialization.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The converters.
    /// </value>
    public IList<JsonConverter> Converters { get; set; }

    /// <summary>
    /// Gets or sets how object references are preserved by the serializer.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The preserve references handling.
    /// </value>
    public PreserveReferencesHandling PreserveReferencesHandling { get; set; }

    /// <summary>
    /// Gets or sets how type name writing and reading is handled by the serializer.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The type name handling.
    /// </value>
    public TypeNameHandling TypeNameHandling { get; set; }

    /// <summary>
    /// Gets or sets how a type name assembly is written and resolved by the serializer.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The type name assembly format.
    /// </value>
    public FormatterAssemblyStyle TypeNameAssemblyFormat { get; set; }

    /// <summary>
    /// Gets or sets how constructors are used during deserialization.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The constructor handling.
    /// </value>
    public ConstructorHandling ConstructorHandling { get; set; }

    /// <summary>
    /// Gets or sets the contract resolver used by the serializer when
    ///             serializing .NET objects to JSON and vice versa.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The contract resolver.
    /// </value>
    public IContractResolver ContractResolver { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="T:Newtonsoft.Json.Serialization.IReferenceResolver"/> used by the serializer when resolving references.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The reference resolver.
    /// </value>
    public IReferenceResolver ReferenceResolver { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="T:System.Runtime.Serialization.SerializationBinder"/> used by the serializer when resolving type names.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The binder.
    /// </value>
    public SerializationBinder Binder { get; set; }

    /// <summary>
    /// Gets or sets the error handler called during serialization and deserialization.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The error handler called during serialization and deserialization.
    /// </value>
    public EventHandler<ErrorEventArgs> Error { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="T:System.Runtime.Serialization.StreamingContext"/> used by the serializer when invoking serialization callback methods.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The context.
    /// </value>
    public StreamingContext Context { get; set; }

    /// <summary>
    /// Gets or sets the maximum depth allowed when reading JSON. Reading past this depth will throw a <see cref="T:Newtonsoft.Json.JsonReaderException"/>.
    /// 
    /// </summary>
    public int? MaxDepth
    {
      get
      {
        return this._maxDepth;
      }
      set
      {
        int? nullable = value;
        if ((nullable.GetValueOrDefault() > 0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
          throw new ArgumentException("Value must be positive.", "value");
        this._maxDepth = value;
        this._maxDepthSet = true;
      }
    }

    /// <summary>
    /// Indicates how JSON text output is formatted.
    /// 
    /// </summary>
    public Formatting Formatting
    {
      get
      {
        return this._formatting ?? Formatting.None;
      }
      set
      {
        this._formatting = new Formatting?(value);
      }
    }

    /// <summary>
    /// Get or set how dates are written to JSON text.
    /// 
    /// </summary>
    public DateFormatHandling DateFormatHandling
    {
      get
      {
        return this._dateFormatHandling ?? DateFormatHandling.IsoDateFormat;
      }
      set
      {
        this._dateFormatHandling = new DateFormatHandling?(value);
      }
    }

    /// <summary>
    /// Get or set how <see cref="T:System.DateTime"/> time zones are handling during serialization and deserialization.
    /// 
    /// </summary>
    public DateTimeZoneHandling DateTimeZoneHandling
    {
      get
      {
        return this._dateTimeZoneHandling ?? DateTimeZoneHandling.RoundtripKind;
      }
      set
      {
        this._dateTimeZoneHandling = new DateTimeZoneHandling?(value);
      }
    }

    /// <summary>
    /// Get or set how date formatted strings, e.g. "\/Date(1198908717056)\/" and "2012-03-21T05:40Z", are parsed when reading JSON.
    /// 
    /// </summary>
    public DateParseHandling DateParseHandling
    {
      get
      {
        return this._dateParseHandling ?? DateParseHandling.DateTime;
      }
      set
      {
        this._dateParseHandling = new DateParseHandling?(value);
      }
    }

    /// <summary>
    /// Gets or sets the culture used when reading JSON. Defaults to <see cref="P:System.Globalization.CultureInfo.InvariantCulture"/>.
    /// 
    /// </summary>
    public CultureInfo Culture
    {
      get
      {
        return this._culture ?? JsonSerializerSettings.DefaultCulture;
      }
      set
      {
        this._culture = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether there will be a check for additional content after deserializing an object.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <c>true</c> if there will be a check for additional content after deserializing an object; otherwise, <c>false</c>.
    /// 
    /// </value>
    public bool CheckAdditionalContent
    {
      get
      {
        return this._checkAdditionalContent ?? false;
      }
      set
      {
        this._checkAdditionalContent = new bool?(value);
      }
    }

    static JsonSerializerSettings()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonSerializerSettings"/> class.
    /// 
    /// </summary>
    public JsonSerializerSettings()
    {
      this.ReferenceLoopHandling = ReferenceLoopHandling.Error;
      this.MissingMemberHandling = MissingMemberHandling.Ignore;
      this.ObjectCreationHandling = ObjectCreationHandling.Auto;
      this.NullValueHandling = NullValueHandling.Include;
      this.DefaultValueHandling = DefaultValueHandling.Include;
      this.PreserveReferencesHandling = PreserveReferencesHandling.None;
      this.TypeNameHandling = TypeNameHandling.None;
      this.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
      this.Context = JsonSerializerSettings.DefaultContext;
      this.Converters = (IList<JsonConverter>) new List<JsonConverter>();
    }
  }
}
