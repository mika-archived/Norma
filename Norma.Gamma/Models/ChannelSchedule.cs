using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class ChannelSchedule
    {
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("date")]
        // [JsonConverter(typeof(IsoDateTimeConverter))]
        public string Date { get; set; }

        [JsonProperty("slots")]
        public Slot[] Slots { get; set; }
    }
}