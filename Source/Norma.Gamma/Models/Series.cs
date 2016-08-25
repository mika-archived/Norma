using System;

using Newtonsoft.Json;

using Norma.Gamma.Converters;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.46")]
    public class Series
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("themeColor")]
        public ThemeColor ThemeColor { get; set; }

        [JsonProperty("updatedAt")]
        [JsonConverter(typeof(UnixTimeDateTimeConverter))]
        public DateTime UpdatedAt { get; set; }
    }
}