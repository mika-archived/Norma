using System.Collections.Specialized;

namespace Norma.Models
{
    internal class NetworkEventArgs
    {
        public string Url { get; }

        public NameValueCollection Headers { get; }

        public string Contents { get; }

        public NetworkEventArgs(string url, NameValueCollection headers, string contents)
        {
            Url = url;
            Headers = headers;
            Contents = contents;
        }
    }
}