using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceInterface;

namespace HTA.Adventures.API.ServiceInterface
{
    public class AdventureReviewService : RestServiceBase<AdventureReview>
    {
        public IAdventureReviewRepository AdventureReviewRepository { get; set; }

        public override object OnPost(AdventureReview request)
        {
            return AdventureReviewRepository.SaveAdventureReview(request);
        }

        public override object OnGet(AdventureReview request)
        {
            if (!string.IsNullOrEmpty(request.Id))
            {
                return AdventureReviewRepository.GetAdventureReviewById(request.Id);
            }
            return AdventureReviewRepository.GetAdventureReviews();
        }
    }
}