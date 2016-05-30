using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class ThemeColor
    {
        [JsonProperty("primary")]
        public string Primary { get; set; }

        [JsonProperty("secondary")]
        public string Secondary { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("background")]
        public string Background { get; set; }
    }
}