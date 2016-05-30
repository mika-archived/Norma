using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class Series
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("themeColor")]
        public ThemeColor ThemeColor { get; set; }
    }
}