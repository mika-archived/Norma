using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    public class Comments
    {
        [JsonProperty("comments")]
        public Comment[] CommentList { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}