using Newtonsoft.Json;

namespace Norma.Eta.Models.Configurations
{
    public class RootConfig
    {
        [JsonProperty]
        public string AccessToken { get; set; }

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
    }
}