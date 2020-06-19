namespace KesselRun.Business.DataTransferObjects.Weather
{
    public partial class City
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Coord Coord { get; set; }
        public string Country { get; set; }
        public long Population { get; set; }
        public long Timezone { get; set; }
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }
}
