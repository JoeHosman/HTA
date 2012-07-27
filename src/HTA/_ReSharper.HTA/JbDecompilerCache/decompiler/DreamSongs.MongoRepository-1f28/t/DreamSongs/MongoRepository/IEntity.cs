// Type: DreamSongs.MongoRepository.IEntity
// Assembly: DreamSongs.MongoRepository, Version=1.3.2.0, Culture=neutral, PublicKeyToken=null
// Assembly location: E:\Dev\git\HTA\src\HTA\packages\MongoRepository.1.3.2\lib\DreamSongs.MongoRepository.dll

using MongoDB.Bson.Serialization.Attributes;

namespace DreamSongs.MongoRepository
{
  /// <summary>
  /// Entity interface.
  /// 
  /// </summary>
  public interface IEntity
  {
    /// <summary>
    /// Gets or sets the Id of the Entity.
    /// 
    /// </summary>
    /// 
    /// <value>
    /// Id of the Entity.
    /// </value>
    [BsonId]
    string Id { get; set; }
  }
}
