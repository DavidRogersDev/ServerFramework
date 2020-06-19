namespace KesselRun.Business.DataTransferObjects.Weather
{
    public class WeatherDto
    {
        public Coord Coord { get; set; }
        public KesselRun.Business.DataTransferObjects.Weather.Weather[] Weather { get; set; }
        public string Base { get; set; }
        public Main Main { get; set; }
        public long Visibility { get; set; }
        public Wind Wind { get; set; }
        public Clouds Clouds { get; set; }
        public long Dt { get; set; }
        public Sys Sys { get; set; }
        public long Timezone { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public long Cod { get; set; }
    }
}
