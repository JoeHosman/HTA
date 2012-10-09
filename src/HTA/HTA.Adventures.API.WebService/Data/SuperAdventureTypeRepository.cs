using System;
using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.API.WebService.Data
{
    public class SuperAdventureTypeRepository : IAdventureTypeRepository,
                                                IAdventureTypeTemplateRepository,
                                                IAdventureReviewRepository,
                                                IAdventureRegionRepository,
                                                IAdventureLocationRepository
    {

        private static readonly MongoAdventureTypeRepository MongoRepository = new MongoAdventureTypeRepository();
        private static readonly ElasticAdventureLocationRepository ElasticRepository = new ElasticAdventureLocationRepository();


        #region Implementation of IAdventureTypeRepository

        public IList<AdventureType> GetAdventureTypes()
        {
            return MongoRepository.GetAdventureTypes();
        }

        public AdventureType GetAdventureType(string id)
        {
            return MongoRepository.GetAdventureType(id);
        }

        public AdventureType SaveAdventureType(AdventureType adventuretype)
        {
            return MongoRepository.SaveAdventureType(adventuretype);
        }

        public IList<AdventureDataCard> GetTypeDataCards(string typeId)
        {
            return MongoRepository.GetTypeDataCards(typeId);
        }

        #endregion

        #region Implementation of IAdventureTypeTemplateRepository

        public IList<AdventureTypeTemplate> GetTypeTemplateList()
        {
            return MongoRepository.GetTypeTemplateList();
        }

        public AdventureTypeTemplate GetTypeTemplate(string id)
        {
            return MongoRepository.GetTypeTemplate(id);
        }

        public AdventureTypeTemplate SaveTypeTemplate(AdventureTypeTemplate template)
        {
            return MongoRepository.SaveTypeTemplate(template);
        }

        #endregion

        #region Implementation of IAdventureReviewRepository

        public IList<AdventureReview> GetAdventureReviews()
        {
            return MongoRepository.GetAdventureReviews();
        }

        public AdventureReview GetAdventureReviewById(string id)
        {
            return MongoRepository.GetAdventureReviewById(id);
        }

        public AdventureReview SaveAdventureReview(AdventureReview adventureReview)
        {
            return MongoRepository.SaveAdventureReview(adventureReview);
        }

        #endregion

        #region Implementation of IAdventureRegionRepository

        public IList<Region> GetAdventureRegions()
        {
            return MongoRepository.GetAdventureRegions();
        }

        public Region GetAdventureRegion(string id)
        {
            return MongoRepository.GetAdventureRegion(id);
        }

        public Region SaveAdventureRegion(Region region)
        {
            return MongoRepository.SaveAdventureRegion(region);
        }

        #endregion

        #region Implementation of IAdventureLocationRepository

        public IList<AdventureLocation> GetAdventureLocations()
        {
            return MongoRepository.GetAdventureLocations();
        }

        public AdventureLocation GetAdventureLocation(string id)
        {
            return MongoRepository.GetAdventureLocation(id);
        }

        public AdventureLocation SaveAdventureLocation(AdventureLocation model)
        {
            try
            {
                var location = MongoRepository.SaveAdventureLocation(model);
                ElasticRepository.SaveAdventureLocation(location);

                return location;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IList<AdventureLocation> GetRegionAdventureLocations(string regionId)
        {
            return GetRegionAdventureLocations(regionId);
        }

        #endregion
    }
}