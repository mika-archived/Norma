using Newtonsoft.Json;

namespace Norma.Eta.Models.Configurations
{
    public class BrowserConfig
    {
        [JsonProperty]
        public bool DisableChangeChannelByMouseWheel { get; set; }

        [JsonProperty]
        public bool ReloadPageOnBroadcastCommercials { get; set; }

        [JsonProperty]
        public string CustomCss { get; set; }

        public BrowserConfig()
        {
            DisableChangeChannelByMouseWheel = true;
            ReloadPageOnBroadcastCommercials = false;
            CustomCss = @"/* From https://github.com/nakayuki805/AbemaTVChromeExtension/blob/master/onairpage.js */
/* Hide header control */
[class^=""AppContainer__header-container___""] {
  display: none;
}

/* Hide footer control */
[class^=""TVContainer__footer-container___""] {
  display: none;
}

/* Hide side controls */
[class^=""TVContainer__side___""] {
  display: none;
}

/* Hide Twitter panel */
[class^=""TVContainer__twitter-panel___""] {
  transform: translateX(-20px) translateX(-100%);
}
";
        }
    }
}