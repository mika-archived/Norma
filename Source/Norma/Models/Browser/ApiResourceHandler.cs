using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;

using CefSharp;

using Microsoft.Practices.ServiceLocation;

using Norma.Gamma;

namespace Norma.Models.Browser
{
    internal class ApiResourceHandler : ResourceHandler
    {
        private readonly NetworkHandler _networkHandler;

        public ApiResourceHandler()
        {
            _networkHandler = ServiceLocator.Current.GetInstance<NetworkHandler>();
        }

        #region Overrides of ResourceHandler

        public override bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            try
            {
                var httpClient = new HttpClient();
                foreach (var header in request.Headers.AllKeys)
                    if (header.ToLower() != "content-type")
                        httpClient.DefaultRequestHeaders.Add(header, request.Headers.GetValues(header));

                HttpResponseMessage response = null;
                if (request.Method == "OPTIONS")
                    response = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Options, request.Url)).Result;
                else
                    CapturingRequest(request.Url, request.Headers,
                                     request.PostData?.Elements.FirstOrDefault()?.GetBody());

                if (request.Method == "GET")
                    response = httpClient.GetAsync(request.Url).Result;
                if (request.Method == "POST")
                {
                    var content = request.PostData?.Elements.FirstOrDefault()?.GetBody();
                    var httpContent = new StringContent2(content, Encoding.UTF8, "application/json");
                    response = httpClient.PostAsync(request.Url, httpContent).Result;
                }
                if (request.Method == "PUT")
                {
                    var content = request.PostData?.Elements.FirstOrDefault()?.GetBody();
                    var httpContent = new StringContent2(content, Encoding.UTF8, "application/json");
                    response = httpClient.PutAsync(request.Url, httpContent).Result;
                }

                Debug.WriteLine($"APICALL: {request.Url}");
                // 知らん
                if (response == null)
                    throw new NotSupportedException();

                StatusCode = response.StatusCode.GetHashCode();
                MimeType = response.Content.Headers.ContentType.MediaType;
                Stream = response.Content.ReadAsStreamAsync().Result;
                foreach (var header in response.Content.Headers)
                    foreach (var value in header.Value)
                        Headers.Set(header.Key, value);
                foreach (var header in response.Headers)
                    foreach (var value in header.Value)
                        Headers.Set(header.Key, value);

                CapturingResponse(request.Url, Headers, response.Content.ReadAsStringAsync().Result);
                callback.Continue();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return true;
        }

        #endregion

        private void CapturingRequest(string url, NameValueCollection headers, string body)
            => _networkHandler.OnHandlingRequest(new NetworkEventArgs(url, headers, body));

        private void CapturingResponse(string url, NameValueCollection headers, string body)
            => _networkHandler.OnHandlingResponse(new NetworkEventArgs(url, headers, body));
    }
}