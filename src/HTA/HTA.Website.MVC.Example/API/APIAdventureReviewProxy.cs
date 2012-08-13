using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;

namespace HTA.Website.MVC.Example.API
{
    internal class APIAdventureReviewProxy :APIProxyBase, IAdventureReviewRepository
    {
        #region Implementation of IAdventureReviewRepository

        public IList<AdventureReview> GetAdventureReviews()
        {
            var adventureReviews = Client.Get<IList<AdventureReview>>("/Adventure/Reviews");
            return adventureReviews;
        }

        public AdventureReview GetAdventureReviewById(string id)
        {
            var adventureReview = Client.Get<AdventureReview>("/Adventure/Reviews/" + id);
            return adventureReview;
        }

        public AdventureReview SaveAdventureReview(AdventureReview adventureReview)
        {
            var saveAdventureReview = Client.Post<AdventureReview>("/Adventure/Reviews/", adventureReview);
            return saveAdventureReview;
        }

        #endregion
    }
}