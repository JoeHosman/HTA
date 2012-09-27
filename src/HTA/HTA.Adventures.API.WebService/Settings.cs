using System.Configuration;

namespace HTA.Adventures.API.WebService
{
    public class Settings
    {
        public static string ElasticLocationServer
        {
            get { return ConfigurationManager.AppSettings["ElasticSearchLocationServer"]; }
        }

        public static string DefaultLocationSearchRange
        {
            get
            {
                var value = ConfigurationManager.AppSettings["DefaultLocationSearcRange"];

                return string.IsNullOrEmpty(value) ? "15mi" : value;
            }
        }
    }
}
