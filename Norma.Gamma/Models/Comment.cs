using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class Comment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("createdAtMs")]
        public int CreatedAtMs { get; set; }
    }
}