using System;
using System.Linq;
using System.Collections.Generic;
using DreamSongs.MongoRepository;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using MongoDB.Bson;

namespace HTA.Adventures.API.WebService.Data
{
    public class MongoAdventureTypeRepository : IAdventureTypeRepository, IAdventureTypeTemplateRepository
    {
        private static readonly MongoRepository<AdventureType> AdventureTypeRepository = new MongoRepository<AdventureType>();
        private static readonly MongoRepository<AdventureTypeTemplate> AdventureTypeTemplateRepository = new MongoRepository<AdventureTypeTemplate>();

        #region Implementation of IAdventureTypeRepository

        public IList<AdventureType> GetAdventureTypes()
        {
            return AdventureTypeRepository.All().ToList();
        }

        public AdventureType GetAdventureType(string id)
        {
            // First check if we're looking by Id
            ObjectId oId;
            if (ObjectId.TryParse(id, out oId))
            {
                var adventureType = AdventureTypeRepository.GetById(oId.ToString());
                return adventureType;
            }

            id = id.Trim().ToLower();
            // Check by name
            var byName = AdventureTypeRepository.GetSingle(t => t.Name.ToLower() == id);

            return byName;
        }

        public AdventureType SaveAdventureType(AdventureType adventuretype)
        {
            return string.IsNullOrEmpty(adventuretype.Id) ?
                AdventureTypeRepository.Add(adventuretype) :  // Add new one  &
                AdventureTypeRepository.Update(adventuretype); // update existing one
        }

        public IList<AdventureDataCard> GetTypeDataCards(string id)
        {
            var adventureType = GetAdventureType(id);

            if (adventureType == null)
                return null;

            var returnValue = new List<AdventureDataCard>();
            foreach (var typeTemplate in adventureType.DataCardTemplates)
            {
                returnValue.AddRange(typeTemplate.DataCards);
            }


            return returnValue;
        }

        #endregion

        #region Implementation of IAdventureTypeTemplateRepository

        public IList<AdventureTypeTemplate> GetTypeTemplateList()
        {
            return AdventureTypeTemplateRepository.All().ToList();
        }

        public AdventureTypeTemplate GetTypeTemplate(string id)
        {
            return AdventureTypeTemplateRepository.GetById(id);
        }

        public AdventureTypeTemplate SaveTypeTemplate(AdventureTypeTemplate template)
        {
            return string.IsNullOrEmpty(template.Id) ?
               AdventureTypeTemplateRepository.Add(template) :  // Add new one  &
               AdventureTypeTemplateRepository.Update(template); // update existing one
        }

        #endregion
    }
}