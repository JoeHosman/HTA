using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types.Responses
{
    public class NearBySearchResponse : IHasResponseStatus
    {
        public NearBySearch Request { get; set; }

        public NearBySearchResponse(NearBySearch request)
        {
            Request = request;
        }

        public List<Spot> Result { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}