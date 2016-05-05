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
            request.Headers.Add("Authorization", "Bearer " + _abemaApi.AccessToken);
            return base.SendAsync(request, cancellationToken);
        }

        #endregion
    }
}