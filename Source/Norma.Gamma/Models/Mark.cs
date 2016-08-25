using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.46")]
    public class Mark
    {
        [JsonProperty("first")]
        public bool IsFirst { get; set; }

        [JsonProperty("live")]
        public bool IsLive { get; set; }
    }
}