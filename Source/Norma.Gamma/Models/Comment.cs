using System;

using Newtonsoft.Json;

using Norma.Gamma.Converters;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.46")]
    public class Comment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("createdAtMs")]
        [JsonConverter(typeof(UnixTimeDateTimeConverter))]
        public DateTime CreatedAtMs { get; set; }
    }
}