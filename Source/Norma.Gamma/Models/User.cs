using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class User
    {
        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}