using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.41")]
    public class Episode
    {
        [JsonProperty("sequence")]
        public string Sequence { get; set; }
    }
}