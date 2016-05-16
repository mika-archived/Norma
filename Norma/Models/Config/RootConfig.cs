using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Norma.Models.Config
{
    internal class RootConfig
    {
        [JsonProperty]
        public string AccessToken { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public AbemaChannel LastViewedChannel { get; set; }

        [JsonProperty]
        public BrowserConfig Browser { get; set; }

        [JsonProperty]
        public OperationConfig Operation { get; set; }

        public RootConfig()
        {
            // Default channel.
            // If saved lastViewdChannel, set new value by Json.NET.
            LastViewedChannel = AbemaChannel.AbemaNews;
        }
    }
}