using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceClient.Web;

namespace HTA.Website.MVC.Example.API
{
    internal class APIAdventureReviewProxy : IAdventureReviewRepository
    {
        private readonly JsonServiceClient _client;

        public APIAdventureReviewProxy()
        {
            _client = new JsonServiceClient("http://localhost:10768/");

        }

        #region Implementation of IAdventureReviewRepository

        public IList<AdventureReview> GetAdventureReviews()
        {
            var adventureReviews = _client.Get<IList<AdventureReview>>("/Adventure/Reviews");
            return adventureReviews;
        }

        public AdventureReview GetAdventureReviewById(string id)
        {
            var adventureReview = _client.Get<AdventureReview>("/Adventure/Reviews/" + id);
            return adventureReview;
        }

        public AdventureReview SaveAdventureReview(AdventureReview adventureReview)
        {
            var saveAdventureReview = _client.Post<AdventureReview>("/Adventure/Reviews/", adventureReview);
            return saveAdventureReview;
        }

        #endregion
    }
}