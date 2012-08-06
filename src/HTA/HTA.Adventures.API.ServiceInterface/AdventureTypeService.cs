using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceInterface;

namespace HTA.Adventures.API.ServiceInterface
{
    public class AdventureTypeTemplateService : RestServiceBase<AdventureTypeTemplate>
    {
        public IAdventureTypeTemplateRepository AdventureTypeRepository { get; set; }

        public override object OnPost(AdventureTypeTemplate request)
        {
            return AdventureTypeRepository.SaveTypeTemplate(request);
        }

        private AdventureTypeTemplate GetTemplate(string id)
        {
            return AdventureTypeRepository.GetTypeTemplate(id);
        }

        public override object OnGet(AdventureTypeTemplate request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.Id))
                {
                    return GetTemplate(request.Id);
                }
                return AdventureTypeRepository.GetTypeTemplateList();
            }
            catch (System.Exception exception)
            {

                string ex = exception.Message;

                return null;
            }
        }
    }
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
            try
            {
                if (!string.IsNullOrEmpty(request.Id))
                {
                    return GetAdventureType(request.Id);
                }
                return AdventureTypeRepository.GetAdventureTypes();
            }
            catch (System.Exception exception)
            {

                string ex = exception.Message;

                return null;
            }
        }
    }

    public class AdventureTypeDataCardService : RestServiceBase<AdventureTypeDataCards>
    {
        public IAdventureTypeRepository AdventureTypeRepository { get; set; }
        public override object OnGet(AdventureTypeDataCards request)
        {
            var response = new AdventureTypeDataCardsResponse() { Id = request.Id };
            response.DataCards = AdventureTypeRepository.GetTypeDataCards(request.Id);

            return response;
        }
    }

}