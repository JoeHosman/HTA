using System.Collections.Generic;
using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using Nest;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types
{
    [ElasticType(Name = "region", IdProperty = "id")]
    [DataContract]
    [RestService("/Adventure/Regions")]
    [RestService("/Adventure/Regions/{Id}")]
    [CollectionName("AdventureRegions")]
    public class AdventureRegion : AdventureSpot
    {
        public AdventureRegion()
            : base()
        {

        }
        public AdventureRegion(LocationPoint locationPoint, string name)
            : base(locationPoint, name)
        {
        }

        public AdventureLocation CreateLocation()
        {
            return new AdventureLocation(this);
        }
    }

    public class AdventureRegionResponse : IHasResponseStatus
    {
        public AdventureRegionResponse(AdventureRegion request)
        {
            Request = request;
        }

        public AdventureRegion Request { get; set; }

        public AdventureRegion Region { get; set; }

        public IList<AdventureLocation> Locations { get; set; }

        #region Implementation of IHasResponseStatus

        public ResponseStatus ResponseStatus { get; set; }

        #endregion
    }
}