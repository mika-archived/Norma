using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.46")]
    public class SlotAudience
    {
        [JsonProperty("slotId")]
        public string SlotId { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("viewCount")]
        public int ViewCount { get; set; }

        [JsonProperty("commentCount")]
        public int CommentCount { get; set; }
    }
}