using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Models
{
    public interface IAdventureDataCardRepository
    {
        AdventureDataCard GetAdventureDataCardById(string dataCardId);
        IList<AdventureDataCard> GetAdventureDataCardsByReviewId(string reviewId);

        AdventureDataCard SaveAdventureDataCard(AdventureDataCard item);
    }
}