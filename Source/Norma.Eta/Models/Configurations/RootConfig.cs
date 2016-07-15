using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Norma.Eta.Models.Configurations
{
    public class RootConfig
    {
        [JsonProperty]
        public string AccessToken { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        [Obsolete("1.6", false)]
        public AbemaChannel LastViewedChannel { get; set; }

        [JsonProperty]
        public string LastViewedChannelStr { get; set; }

        [JsonProperty]
        public BrowserConfig Browser { get; set; }

        [JsonProperty]
        public OperationConfig Operation { get; set; }

        [JsonProperty]
        public OthersConfig Others { get; set; }

        [JsonProperty]
        public InternalConfig Internal { get; set; }

        public RootConfig()
        {
            // Default channel.
            // If saved lastViewdChannel, set new value by Json.NET.
            LastViewedChannel = AbemaChannel.AbemaNews;
        }
    }
}