using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Adventures.Models.Types.Responses;

namespace HTA.Website.MVC.Example.API
{
    internal class APIAdventureReviewProxy : APIProxyBase, IAdventureReviewRepository
    {
        #region Implementation of IAdventureReviewRepository

        public IList<AdventureReview> GetAdventureReviews()
        {
            var response = Client.Get<AdventureReviewGetResponse>("/Adventure/Reviews");

            return response.AdventureReviews;
        }

        public AdventureReview GetAdventureReviewById(string id)
        {
            var response = Client.Get<AdventureReviewGetResponse>("/Adventure/Reviews/" + id);
            return response.AdventureReviews[0];
        }

        public AdventureReview SaveAdventureReview(AdventureReview adventureReview)
        {
            var response = Client.Post<AdventureReviewSaveResponse>("/Adventure/Reviews/", adventureReview);
            return response.AdventureReview;
        }

        #endregion
    }
}