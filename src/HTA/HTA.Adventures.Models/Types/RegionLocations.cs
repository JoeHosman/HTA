using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ServiceStack.ServiceHost;

namespace HTA.Adventures.Models.Types
{
    [RestService("/Adventure/Region/Locations/{RegionId}")]
    [DataContract]
    public class AdventureRegionAdventureLocationsRequest
    {
        [DataMember]
        public string RegionId { get; set; }

    }
}
