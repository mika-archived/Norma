using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class Profile
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("createdAt")]
        // [JsonConverter(typeof(Newtonsoft.Json.Converters.))]
        public /* DateTime */ int CreatedAt { get; set; }
    }
}