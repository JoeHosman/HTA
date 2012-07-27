// Type: DreamSongs.MongoRepository.Entity
// Assembly: DreamSongs.MongoRepository, Version=1.3.2.0, Culture=neutral, PublicKeyToken=null
// Assembly location: E:\Dev\git\HTA\src\HTA\packages\MongoRepository.1.3.2\lib\DreamSongs.MongoRepository.dll

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace DreamSongs.MongoRepository
{
  /// <summary>
  /// Abstract Entity for all the BusinessEntities.
  /// 
  /// </summary>
  [DataContract]
  [BsonIgnoreExtraElements(Inherited = true)]
  public abstract class Entity : IEntity
  {
    /// <summary>
    /// Gets or sets the id for this object (the primary record for an entity).
    /// 
    /// </summary>
    /// 
    /// <value>
    /// The id for this object (the primary record for an entity).
    /// </value>
    [DataMember]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
  }
}
