using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class ProvidedInfo
    {
        [JsonProperty("thumbImg")]
        public string ThumbImg { get; set; }

        [JsonProperty("sceneThumbImgs")]
        public string[] SceneThumbImgs { get; set; }
    }
}