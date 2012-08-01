using HTA.Adventures.Models.Types;
using ServiceStack.ServiceHost;

namespace HTA.Adventures.API.WebService.App_Start
{
    [RestService("/Adventure/Locations/{LatLon}", Verbs = "GET")]
    public class NearByAdventureLocations : NearBySearch
    {
    }

}