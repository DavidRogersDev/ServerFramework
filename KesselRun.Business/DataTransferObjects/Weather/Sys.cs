namespace KesselRun.Business.DataTransferObjects.Weather
{
    public class Sys
    {
        public string Pod { get; set; }

        public long Type { get; set; }
        public long Id { get; set; }
        public string Country { get; set; }
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }
}