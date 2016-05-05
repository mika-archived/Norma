using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class Slot
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("startAt")]
        public int StartAt { get; set; }

        [JsonProperty("endAt")]
        public int EndAt { get; set; }

        [JsonProperty("program")]
        public Program[] Programs { get; set; }

        [JsonProperty("tableStartAt")]
        public int TableStartAt { get; set; }

        [JsonProperty("tableEndAt")]
        public int TableEndAt { get; set; }

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
        public int TimeshiftEndAt { get; set; }
    }
}