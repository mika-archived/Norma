using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Norma.Models.Config
{
    internal class ConfigRoot
    {
        [JsonProperty]
        public string AccessToken { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public AbemaChannel LastViewedChannel { get; set; }

        public ConfigRoot()
        {
            // Default channel.
            // If saved lastViewdChannel, set new value by Json.NET.
            LastViewedChannel = AbemaChannel.AbemaNews;
        }
    }
}