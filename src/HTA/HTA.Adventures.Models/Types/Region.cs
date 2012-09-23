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
    public class Region : Spot
    {
        public Region()
            : base()
        {

        }
        public Region(GeoPoint geoPoint, string name)
            : base(geoPoint, name)
        {
        }


        public static Region CreateNewRegion
        {
            get { return new Region() { Name = "<Create New Region>", Id = string.Empty }; }
        }

        public Location CreateLocation()
        {
            return new Location(this);
        }
    }

    [DataContract]
    public class AdventureRegionResponse : IHasResponseStatus
    {
        public AdventureRegionResponse(Region request)
        {
            Request = request;
        }

        [DataMember]
        public Region Request { get; set; }

        [DataMember]
        public Region Region { get; set; }

        [DataMember]
        public IList<Location> Locations { get; set; }

        #region Implementation of IHasResponseStatus

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        #endregion
    }
}