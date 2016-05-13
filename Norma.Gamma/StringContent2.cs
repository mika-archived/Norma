using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Norma.Gamma
{
    internal class StringContent2 : StringContent
    {
        public StringContent2(string content) : this(content, null, null)
        {

        }

        public StringContent2(string content, Encoding encoding) : base(content, encoding, null)
        {

        }

        public StringContent2(string content, Encoding encoding, string mediaType) : base(content, encoding, mediaType)
        {
            Headers.ContentType = new MediaTypeHeaderValue(mediaType ?? "text/plain");
        }
    }
}