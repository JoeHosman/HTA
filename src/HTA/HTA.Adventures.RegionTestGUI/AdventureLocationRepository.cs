using System.Collections.Generic;
using DreamSongs.MongoRepository;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using Nest;

namespace HTA.Adventures.RegionTestGUI
{
    public class AdventureLocationRepository : IAdventureLocationRepository
    {
        public AdventureLocationRepository()
        {
            var setting = new ConnectionSettings("office.mtctickets.com", 9200);
            setting.SetDefaultIndex("pins");
            client = new ElasticClient(setting);
        }

        static readonly MongoRepository<AdventureSpot> MongoAdventureLocationRepository = new MongoRepository<AdventureSpot>();
        private readonly ElasticClient client;
        public IList<AdventureLocation> GetAdventureLocations()
        {
            throw new System.NotImplementedException();
        }
    }
}