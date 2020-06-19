using System;
using System.Collections.Generic;

namespace KesselRun.Business.DataTransferObjects.Media
{
    public class TvShow
    {
        public string Title { get; set; }
        public int Season { get; set; }
        public int TotalSeasons { get; set; }
        public string Response { get; set; }


        public ICollection<Episode> Episodes { get; set; }
    }
}
