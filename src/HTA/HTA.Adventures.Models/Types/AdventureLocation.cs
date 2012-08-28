using System.Collections.Generic;
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
    public class AdventureLocation : AdventureSpot
    {
        public AdventureLocation() : base() { AdventureRegion = new AdventureRegion(); }
        public AdventureLocation(LocationPoint locationPoint, string name)
            : base(locationPoint, name)
        {
        }

        public AdventureLocation(AdventureRegion adventureRegion)
        {
            base.LocationPoint = adventureRegion.LocationPoint;
            base.Address = adventureRegion.Address;
            base.Name = adventureRegion.Name;
            base.Picture = adventureRegion.Picture;
            AdventureRegion = adventureRegion;
        }

        [ElasticProperty(Name = "region")]
        [DataMember]
        public AdventureRegion AdventureRegion { get; set; }

    }

    public class AdventureLocationResponse : IHasResponseStatus
    {
        public AdventureLocationResponse(AdventureLocation request)
        {
            Request = request;
        }

        public AdventureLocation Request { get; set; }

        public AdventureRegion Region { get; set; }

        public AdventureLocation Location { get; set; }

        #region Implementation of IHasResponseStatus

        public ResponseStatus ResponseStatus { get; set; }

        #endregion
    }
}