using System;

using Newtonsoft.Json;

using Norma.Gamma.Converters;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.41")]
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