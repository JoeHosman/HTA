using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Models
{
    public interface IAdventureTypeRepository
    {
        IList<AdventureType> GetAdventureTypes();
        AdventureType GetAdventureType(string id);
        AdventureType SaveAdventureType(AdventureType adventuretype);

        IList<AdventureDataCard> GetTypeDataCards(string typeId);
    }
}