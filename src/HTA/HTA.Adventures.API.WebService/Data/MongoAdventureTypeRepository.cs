using System;
using System.Linq;
using System.Collections.Generic;
using DreamSongs.MongoRepository;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using MongoDB.Bson;

namespace HTA.Adventures.API.WebService.Data
{
    public class MongoAdventureTypeRepository : IAdventureTypeRepository,
                                                IAdventureTypeTemplateRepository,
                                                IAdventureReviewRepository,
                                                IAdventureRegionRepository,
                                                IAdventureLocationRepository
    {
        private static readonly MongoRepository<AdventureRegion> AdventureRegionRepository = new MongoRepository<AdventureRegion>();
        private static readonly MongoRepository<AdventureReview> AdventureReviewRepository = new MongoRepository<AdventureReview>();
        private static readonly MongoRepository<AdventureType> AdventureTypeRepository = new MongoRepository<AdventureType>();
        private static readonly MongoRepository<AdventureTypeTemplate> AdventureTypeTemplateRepository = new MongoRepository<AdventureTypeTemplate>();
        private static readonly MongoRepository<AdventureLocation> AdventureLocationRepository = new MongoRepository<AdventureLocation>();

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

        public IList<AdventureDataCard> GetTypeDataCards(string typeId)
        {
            var adventureType = GetAdventureType(typeId);

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

        #region Implementation of IAdventureReviewRepository

        public IList<AdventureReview> GetAdventureReviews()
        {
            return AdventureReviewRepository.All().ToList();
        }

        public AdventureReview GetAdventureReviewById(string id)
        {
            return AdventureReviewRepository.GetById(id);
        }

        public AdventureReview SaveAdventureReview(AdventureReview adventureReview)
        {
            return string.IsNullOrEmpty(adventureReview.Id) ?
               AdventureReviewRepository.Add(adventureReview) :  // Add new one  &
               AdventureReviewRepository.Update(adventureReview); // update existing one
        }

        #endregion

        #region Implementation of IAdventureRegionRepository

        public IList<AdventureRegion> GetAdventureRegions()
        {
            return AdventureRegionRepository.All().ToList();
        }

        public AdventureRegion GetAdventureRegion(string id)
        {
            return AdventureRegionRepository.GetById(id);
        }

        public AdventureRegion SaveAdventureRegion(AdventureRegion adventureRegion)
        {
            return (string.IsNullOrEmpty(adventureRegion.Id)) ?
                AdventureRegionRepository.Add((AdventureRegion)adventureRegion) :
            AdventureRegionRepository.Update((AdventureRegion)adventureRegion);
        }

        #endregion

        #region Implementation of IAdventureLocationRepository

        public IList<AdventureLocation> GetAdventureLocations()
        {
            return AdventureLocationRepository.All().ToList();
        }

        public AdventureLocation GetAdventureLocation(string id)
        {
            return AdventureLocationRepository.GetById(id);
        }

        public AdventureLocation SaveAdventureReview(AdventureLocation model)
        {
            return (string.IsNullOrEmpty(model.Id))
                       ? AdventureLocationRepository.Add(model)
                       : AdventureLocationRepository.Update(model);
        }

        #endregion
    }
}