using ServiceStack.ServiceClient.Web;

namespace HTA.Website.MVC.Example.API
{
    internal class APIProxyBase
    {
        protected JsonServiceClient Client;

        public APIProxyBase()
        {
            Client = new JsonServiceClient("http://localhost:10768/");

        }
    }
}