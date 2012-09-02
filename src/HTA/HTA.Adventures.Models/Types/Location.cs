using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using Nest;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types
{
    [ElasticType(Name = "location", IdProperty = "id")]
    [DataContract]
    [RestService("/Adventure/Locations")]
    [RestService("/Adventure/Locations/{Id}")]
    [CollectionName("AdventureLocations")]
    public class Location : Spot
    {
        public Location()
        { Region = new Region(); }
        public Location(GeoPoint geoPoint, string name)
            : base(geoPoint, name)
        {
        }

        public Location(Region region)
        {
            base.Point = region.Point;
            base.Address = region.Address;
            base.Name = region.Name;
            base.Picture = region.Picture;
            Region = region;
        }

        [ElasticProperty(Name = "region")]
        [DataMember]
        public Region Region { get; set; }

    }

    [DataContract]
    public class AdventureLocationResponse : IHasResponseStatus
    {
        public AdventureLocationResponse(Location request)
        {
            Request = request;
        }

        [DataMember]
        public Location Request { get; set; }

        [DataMember]
        public Region Region { get; set; }

        [DataMember]
        public Location Location { get; set; }

        #region Implementation of IHasResponseStatus

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        #endregion
    }
}