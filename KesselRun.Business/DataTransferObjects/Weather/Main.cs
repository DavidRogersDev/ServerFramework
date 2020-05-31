namespace KesselRun.Business.DataTransferObjects.Weather
{
    public class Main
    {
        public double Temp { get; set; }
        public double FeelsLike { get; set; }
        public double TempMin { get; set; }
        public long TempMax { get; set; }
        public long Pressure { get; set; }
        public long SeaLevel { get; set; }
        public long GrndLevel { get; set; }
        public long Humidity { get; set; }
        public double TempKf { get; set; }
    }
}