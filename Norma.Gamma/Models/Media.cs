using System;

using Newtonsoft.Json;

using Norma.Gamma.Converters;

namespace Norma.Gamma.Models
{
    public class Media
    {
        [JsonProperty("channels")]
        public Channel[] Channels { get; set; }

        [JsonProperty("channelSchedules")]
        public ChannelSchedule[] ChannelSchedules { get; set; }

        [JsonProperty("availableDates")]
        [JsonConverter(typeof(DateDateTimeConverter))]
        public DateTime[] AvailableDates { get; set; }
    }
}