using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Norma.Gamma.Models
{
    public class ChannelSchedule
    {
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("date")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty("slots")]
        public Slot[] Slots { get; set; }
    }
}