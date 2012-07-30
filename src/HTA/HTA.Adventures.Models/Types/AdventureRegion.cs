using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DreamSongs.MongoRepository;
using Nest;

namespace HTA.Adventures.Models.Types
{
    /// <summary>
    /// A region where adventures happen
    /// </summary>
    public partial class AdventureRegion : Entity
    {
        public string Name { get; set; }
        public string PathName { get; set; }
        public string NearBy { get; set; }

    }

    public class GeoRange
    {
        public int Range { get; set; }
    }


    public class Geo
    {
        [ElasticProperty(Name = "location")]
        public Location Location { get; set; }
    }
    public class Location
    {
        [ElasticProperty(Name = "lon")]
        public double Lon { get; set; }
        [ElasticProperty(Name = "lat")]
        public double Lat { get; set; }

    }
    [ElasticType(Name = "region", IdProperty = "id")]
    [DataContract]
    public partial class AdventureLocation : Entity
    {
        [ElasticProperty(Name = "geo")]
        [DataMember]
        public Geo Geo { get; set; }

        [ElasticProperty(Name = "name")]
        [DataMember]
        public string Name { get; set; }

        [ElasticProperty(Name = "address")]
        [DataMember]
        public string Address { get; set; }

        public AdventureLocation(Geo location, string name, string address)
        {
            Geo = location;
            this.Name = name;
            this.Address = address;
        }
    }
}
