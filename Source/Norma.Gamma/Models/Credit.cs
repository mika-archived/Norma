using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.46")]
    public class Credit
    {
        [JsonProperty("casts")]
        public string[] Cast { get; set; }

        [JsonProperty("crews")]
        public string[] Crews { get; set; }

        [JsonProperty("copyrights")]
        public string[] Copyrights { get; set; }
    }
}