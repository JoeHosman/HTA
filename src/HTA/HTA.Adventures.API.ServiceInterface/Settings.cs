using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace HTA.Adventures.API.ServiceInterface
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
