using System.Text.Json.Serialization;

namespace KesselRun.Business.DataTransferObjects.Media
{
    public class Episode
    {
        public string Title { get; set; }
        public string Released { get; set; }
        [JsonPropertyName("episode")]
        public int EpisodeNr { get; set; }
        public string ImdbRating { get; set; }
        public string ImdbID { get; set; }
    }
}
