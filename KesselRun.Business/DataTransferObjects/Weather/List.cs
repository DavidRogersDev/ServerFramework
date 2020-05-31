using System;

namespace KesselRun.Business.DataTransferObjects.Weather
{
    public partial class List
    {
        public long Dt { get; set; }
        public Main Main { get; set; }
        public Weather[] Weather { get; set; }
        public Clouds Clouds { get; set; }
        public Wind Wind { get; set; }
        public Sys Sys { get; set; }
        public DateTimeOffset DtTxt { get; set; }
        public Rain Rain { get; set; }
    }
}
