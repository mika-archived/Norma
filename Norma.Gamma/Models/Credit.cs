using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class Credit
    {
        [JsonProperty("casts")]
        public string[] Cast { get; set; }

        [JsonProperty("crews")]
        public string[] Crews { get; set; }

        [JsonProperty("copyrights")]
        public string[] Copyrights { get; set; }

        [JsonProperty("series")]
        public Series Series { get; set; }

        [JsonProperty("providedInfo")]
        public ProvidedInfo ProvidedInfo { get; set; }
    }
}