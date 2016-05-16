using Newtonsoft.Json;

namespace Norma.Models.Config
{
    internal class BrowserConfig
    {
        [JsonProperty]
        public bool HiddenHeaderControls { get; set; }

        [JsonProperty]
        public bool HiddenFooterControls { get; set; }

        [JsonProperty]
        public bool HiddenSideControls { get; set; }

        [JsonProperty]
        public bool DisableChangeChannelByMouseWheel { get; set; }

        [JsonProperty]
        public bool ReloadPageOnBroadcastCommercials { get; set; }

        public BrowserConfig()
        {
            HiddenHeaderControls = true;
            HiddenFooterControls = true;
            HiddenSideControls = true;
            DisableChangeChannelByMouseWheel = true;
            ReloadPageOnBroadcastCommercials = false;
        }
    }
}