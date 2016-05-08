using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class Media
    {
        [JsonProperty("channels")]
        public Channel[] Channels { get; set; }

        [JsonProperty("channelSchedules")]
        public ChannelSchedule[] ChannelSchedules { get; set; }

        [JsonProperty("availableDates")]
        // [JsonConverter(typeof(IsoDateTimeConverter))]
        public string[] AvailableDates { get; set; }
    }
}