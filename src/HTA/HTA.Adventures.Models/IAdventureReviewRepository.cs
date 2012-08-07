using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Models
{
    public interface IAdventureReviewRepository
    {
        IList<AdventureReview> GetAdventureReviews();
        AdventureReview GetAdventureReviewById(string id);
        AdventureReview SaveAdventureReview(AdventureReview adventureReview);
    }
}