using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DreamSongs.MongoRepository;
using HTA.Adventures.Models.Types;
using Nest;
using Newtonsoft.Json;

namespace HTA.Adventures.Models
{
    public interface IAdventureLocationRepository
    {
        void Add(AdventureLocation location);
    }

    public class AdventureLocationRepository : IAdventureLocationRepository
    {
        static readonly MongoRepository<AdventureLocation> MongoAdventureLocationRepository = new MongoRepository<AdventureLocation>();
        public void Add(AdventureLocation location)
        {
            MongoAdventureLocationRepository.Add(location);

            var setting = new ConnectionSettings("office.mtctickets.com", 9200);
            var client = new ElasticClient(setting);

            var result = client.Index(location, "pins", "region", location.Id);

            //it was extra testing
            //string data = JsonConvert.SerializeObject(location);
        }
    }
}
