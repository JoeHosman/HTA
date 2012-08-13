using System.Collections.Generic;
using System.Linq;

namespace HTA.Adventures.Models.Types
{
    public class NearBySearch
    {
        public List<AdventureSpot> Result { get; set; }
        public string LatLon { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Range { get; set; }
        public string Name { get; set; }

        public double[] SplitLatLon()
        {

            var split = LatLon.Split(',');
            double[] output = new double[split.Count()];
            for(int i = 0; i < split.Count(); i++)
            {
                output[i] = double.Parse(split[i]);
            }
            return output;
        }

        public void ValidateRange(string defaultRange)
        {
            if(string.IsNullOrEmpty(Range) || (!Range.EndsWith("mi") && !Range.EndsWith("km")))
            {
                Range = defaultRange;
            }
        }
    }
}