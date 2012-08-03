using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DreamSongs.MongoRepository;
using Nest;
using ServiceStack.ServiceHost;

namespace HTA.Adventures.Models.Types
{
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

    [ElasticType(Name = "adventure_review_ref", IdProperty = "id")]
    [DataContract]
    public class AdventureReviewReference : Entity
    {

    }

    [ElasticType(Name = "adventure_data_card", IdProperty = "id")]
    [DataContract]
    public class AdventureDataCard : Entity
    {
        [ElasticProperty(Name = "title")]
        [DataMember]
        public string Title { get; set; }

        [ElasticProperty(Name = "body")]
        [DataMember]
        public string Body { get; set; }

        [ElasticProperty(Name = "adventure_review")]
        [DataMember]
        public AdventureReviewReference AdventureReview { get; set; }
    }

    [ElasticType(Name = "adventure_type_template", IdProperty = "id")]
    [DataContract]
    public class AdventureTypeTemplate : Entity
    {
        [ElasticProperty(Name = "name")]
        [DataMember]
        public string Name { get; set; }

        [ElasticProperty(Name = "data_cards")]
        [DataMember]
        public List<AdventureDataCard> AdventureDataCards { get; set; }

    }

    [ElasticType(Name = "adventure_type_template_ref", IdProperty = "id")]
    [DataContract]
    public class AdventureTypeTemplateReference : Entity
    {
        [ElasticProperty(Name = "name")]
        [DataMember]
        public string Name { get; set; }
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

        [ElasticProperty(Name = "picture")]
        [DataMember]
        public string Picture { get; set; }


        public AdventureLocation(Geo location, string name, string address, string picture)
        {
            Geo = location;
            this.Name = name;
            this.Address = address;
            Picture = picture;
        }
    }
}
