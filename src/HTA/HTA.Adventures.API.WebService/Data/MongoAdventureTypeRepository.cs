using System;
using System.Linq;
using System.Collections.Generic;
using DreamSongs.MongoRepository;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Adventures.Models.Types.Responses;
using MongoDB.Bson;
using Nest;

namespace HTA.Adventures.API.WebService.Data
{
    public class ElasticAdventureLocationRepository : IAdventureLocationRepository
    {
        public static string ElasticServer { get; set; }
        #region Implementation of IAdventureLocationRepository

        public IList<AdventureLocation> GetAdventureLocations()
        {
            throw new NotImplementedException();
        }

        public AdventureLocation GetAdventureLocation(string id)
        {
            throw new NotImplementedException();
        }

        public AdventureLocation SaveAdventureLocation(AdventureLocation model)
        {
            var setting = new ConnectionSettings(ElasticServer, 9200);
            setting.SetDefaultIndex("adventure");


            var client = new ElasticClient(setting);

            var t = client.Index(model, "adventure", "location", model.Id);

            return null;
        }

        public IList<AdventureLocation> GetRegionAdventureLocations(string regionId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


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
            return GetAdventureRegions();
        }

        public Region GetAdventureRegion(string id)
        {
            return GetAdventureRegion(id);
        }

        public Region SaveAdventureRegion(Region region)
        {
            return SaveAdventureRegion(region);
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


    public class MongoAdventureTypeRepository : IAdventureTypeRepository,
                                                IAdventureTypeTemplateRepository,
                                                IAdventureReviewRepository,
                                                IAdventureRegionRepository,
                                                IAdventureLocationRepository
    {
        private static readonly MongoRepository<Region> AdventureRegionRepository = new MongoRepository<Region>();
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

        public IList<Region> GetAdventureRegions()
        {
            return AdventureRegionRepository.All().ToList();
        }

        public Region GetAdventureRegion(string id)
        {
            return AdventureRegionRepository.GetById(id);
        }

        public Region SaveAdventureRegion(Region region)
        {
            return (string.IsNullOrEmpty(region.Id)) ?
                AdventureRegionRepository.Add((Region)region) :
            AdventureRegionRepository.Update((Region)region);
        }

        #endregion

        #region Implementation of IAdventureLocationRepository

        public IList<AdventureLocation> GetAdventureLocations()
        {
            return AdventureLocationRepository.All().ToList();
        }

        public AdventureLocation GetAdventureLocation(string id)
        {
            var response = AdventureLocationRepository.GetById(id);

            return response;
        }

        public AdventureLocation SaveAdventureLocation(AdventureLocation model)
        {
            var response = new AdventureLocationSaveResponse(model);

            response.AdventureLocation = (string.IsNullOrEmpty(model.Id))
                       ? AdventureLocationRepository.Add(model)
                       : AdventureLocationRepository.Update(model);

            return response.AdventureLocation;
        }

        public IList<AdventureLocation> GetRegionAdventureLocations(string regionId)
        {
            return AdventureLocationRepository.All(l => l.Region.Id == regionId).ToList();
        }

        #endregion
    }
}