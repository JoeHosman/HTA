namespace HTA.Adventures.Models.Types
{
    public class GeoRange
    {
        public double Distance { get; set; }
        public string Unit { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}", Distance.ToString("0.000"), Unit);
        }
    }
}