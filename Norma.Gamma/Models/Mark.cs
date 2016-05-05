using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class Mark
    {
        [JsonProperty("first")]
        public bool IsFirst { get; set; }
    }
}