namespace KesselRun.Business.DataTransferObjects.Weather
{
    public class ForecastDto
    {
        public long Cod { get; set; }
        public long Message { get; set; }
        public long Cnt { get; set; }
        public List[] List { get; set; }
        public City City { get; set; }
    }
}
