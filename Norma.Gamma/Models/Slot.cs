using System;

using Newtonsoft.Json;

using Norma.Gamma.Converters;

namespace Norma.Gamma.Models
{
    public class Slot
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("startAt")]
        [JsonConverter(typeof(UnixTimeDateTimeConverter))]
        public DateTime StartAt { get; set; }

        [JsonProperty("endAt")]
        [JsonConverter(typeof(UnixTimeDateTimeConverter))]
        public DateTime EndAt { get; set; }

        [JsonProperty("program")]
        public Program[] Programs { get; set; }

        [JsonProperty("tableStartAt")]
        [JsonConverter(typeof(UnixTimeDateTimeConverter))]
        public DateTime TableStartAt { get; set; }

        [JsonProperty("tableEndAt")]
        [JsonConverter(typeof(UnixTimeDateTimeConverter))]
        public DateTime TableEndAt { get; set; }

        [JsonProperty("highlight")]
        public string Highlight { get; set; }

        [JsonProperty("tableHighlight")]
        public string TableHighlight { get; set; }

        [JsonProperty("detailHighlight")]
        public string DetailHighlight { get; set; }

        [JsonProperty("displayProgramId")]
        public string DisplayProgramId { get; set; }

        [JsonProperty("mark")]
        public Mark Mark { get; set; }

        [JsonProperty("flags")]
        public Flags Flags { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("timeshiftEndAt")]
        [JsonConverter(typeof(UnixTimeDateTimeConverter))]
        public DateTime TimeshiftEndAt { get; set; }
    }
}