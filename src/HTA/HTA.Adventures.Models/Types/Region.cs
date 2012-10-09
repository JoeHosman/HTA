using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using Nest;
using ServiceStack.ServiceHost;

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
        {

        }
        public Region(GeoPoint geoPoint, string name)
            : base(geoPoint, name)
        {
        }


        public static Region CreateNewRegion
        {
            get { return new Region { Name = "<Create New Region>", Id = string.Empty }; }
        }

        public AdventureLocation CreateLocation()
        {
            return new AdventureLocation(this);
        }

        public static Region CreateRegionFromLocation(AdventureLocation adventureLocation)
        {
            var region = new Region(adventureLocation.Point, adventureLocation.Name)
                             {Address = adventureLocation.Address, Picture = adventureLocation.Picture};

            return region;
        }
    }
}