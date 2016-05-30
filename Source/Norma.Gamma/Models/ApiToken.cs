using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class ApiToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}