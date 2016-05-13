using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class Program
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("episode")]
        public Episode Episode { get; set; }

        [JsonProperty("credit")]
        public Credit Credit { get; set; }

        [JsonProperty("series")]
        public Series Series { get; set; }

        [JsonProperty("providedInfo")]
        public ProvidedInfo ProvidedInfo { get; set; }
    }
}