using Newtonsoft.Json;

namespace Norma.Models.Config
{
    internal class ConfigRoot
    {
        [JsonProperty]
        public string AccessToken { get; set; }
    }
}