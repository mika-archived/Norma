using System;

using Newtonsoft.Json;

using Norma.Gamma.Converters;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.46")]
    public class ProvidedInfo
    {
        [JsonProperty("thumbImg")]
        public string ThumbImg { get; set; }

        [JsonProperty("sceneThumbImgs")]
        public string[] SceneThumbImgs { get; set; }

        [JsonProperty("updatedAt")]
        [JsonConverter(typeof(UnixTimeDateTimeConverter))]
        public DateTime UpdatedAt { get; set; }
    }
}