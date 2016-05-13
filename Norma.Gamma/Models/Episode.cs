using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class Episode
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sequence")]
        public string Sequence { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}