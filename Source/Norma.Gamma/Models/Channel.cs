using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.46")]
    public class Channel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }
    }
}