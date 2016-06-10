using System;

using Newtonsoft.Json;

using Norma.Gamma.Converters;

namespace Norma.Gamma.Models
{
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