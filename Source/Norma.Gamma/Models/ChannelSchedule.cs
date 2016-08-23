using System;

using Newtonsoft.Json;

using Norma.Gamma.Converters;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.46")]
    public class ChannelSchedule
    {
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("date")]
        [JsonConverter(typeof(DateDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty("slots")]
        public Slot[] Slots { get; set; }
    }
}