using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceInterface;

namespace HTA.Adventures.API.ServiceInterface
{
    public class AdventureTypeService : RestServiceBase<AdventureType>
    {
        public IAdventureTypeRepository AdventureTypeRepository { get; set; }

        //private static List<AdventureType> _stuff = new List<AdventureType>();
        public override object OnPost(AdventureType request)
        {
            //if (string.IsNullOrEmpty(request.Id))
            //{
            //    lock (_stuff)
            //    {
            //        request.Id = (_stuff.Count + 1).ToString();
            //        _stuff.Add(request);
            //    }
            //    return request;
            //}

            //_stuff.Remove(GetAdventureType(request.Id));
            //_stuff.Add(request);
            return AdventureTypeRepository.SaveAdventureType(request);
        }

        private AdventureType GetAdventureType(string id)
        {
            return AdventureTypeRepository.GetAdventureType(id);
            //return _stuff.FirstOrDefault(t => t.Id == id || string.Compare(t.Name, id, true) == 0);
        }

        public override object OnGet(AdventureType request)
        {
            if (!string.IsNullOrEmpty(request.Id))
            {
                return GetAdventureType(request.Id);
            }
            return AdventureTypeRepository.GetAdventureTypes();

        }
    }
}