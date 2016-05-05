using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class Flags
    {
        [JsonProperty("share")]
        public bool IsShare { get; set; }

        [JsonProperty("timeshift")]
        public bool IsTimeShift { get; set; }
    }
}