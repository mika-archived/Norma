using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.46")]
    public class Comments
    {
        [JsonProperty("comments")]
        public Comment[] CommentList { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}