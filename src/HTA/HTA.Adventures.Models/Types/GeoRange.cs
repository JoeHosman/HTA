namespace HTA.Adventures.Models.Types
{
    public class GeoRange
    {
        public GeoRange()
        {
            Distance = 1.0;
            Unit = "km";
        }
        public double Distance { get; set; }
        public string Unit { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}", Distance.ToString("0.000"), Unit);
        }
    }
}