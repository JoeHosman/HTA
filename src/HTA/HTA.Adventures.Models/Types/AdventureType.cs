using System.Collections.Generic;
using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using Nest;
using ServiceStack.ServiceHost;

namespace HTA.Adventures.Models.Types
{
    [RestService("/Adventure/Types")]
    [DataContract]
    public class AdventureType : Entity
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string IconLink { get; set; }

        [DataMember]
        public string Name { get; set; }


        [DataMember]
        public string PhotoLink { get; set; }

        [DataMember]
        public IList<AdventureTypeTemplate> DataCardTemplates { get; set; }
    }
}