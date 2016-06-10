using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Norma.Gamma
{
    internal class AbemaAuthorizationHandler : HttpClientHandler
    {
        private readonly AbemaTv _abemaApi;

        public AbemaAuthorizationHandler(AbemaTv abemaApi)
        {
            _abemaApi = abemaApi;
        }

        #region Overrides of HttpClientHandler

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_abemaApi.AccessToken))
                request.Headers.Add("Authorization", "Bearer " + _abemaApi.AccessToken);
            request.Headers.Add("User-Agent",
                                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.63 Safari/537.36");
            return base.SendAsync(request, cancellationToken);
        }

        #endregion
    }
}