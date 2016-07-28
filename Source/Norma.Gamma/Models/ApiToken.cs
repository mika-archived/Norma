using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.41")]
    public class ApiToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}