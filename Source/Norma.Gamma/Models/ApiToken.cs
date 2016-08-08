using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.42")]
    public class ApiToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}