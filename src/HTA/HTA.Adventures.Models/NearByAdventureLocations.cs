using HTA.Adventures.Models.Types;
using ServiceStack.ServiceHost;

namespace HTA.Adventures.Models
{

    [RestService("/Adventure/LocationSearch/{LatLon}", Verbs = "GET")]
    public class NearByAdventureLocations : NearBySearch
    {
    }
}