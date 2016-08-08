using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.42")]
    public class Profile
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("createdAt")]
        // [JsonConverter(typeof(Newtonsoft.Json.Converters.))]
        public /* DateTime */ int CreatedAt { get; set; }
    }
}