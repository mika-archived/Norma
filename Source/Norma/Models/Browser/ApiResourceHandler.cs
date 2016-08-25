using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public override bool ProcessRequestAsync(IRequest taskRequest, ICallback callback)
        {
            var headers = taskRequest.Headers;
            var url = taskRequest.Url;
            var method = taskRequest.Method;
            var postData = taskRequest.PostData?.Elements.FirstOrDefault()?.GetBody();
            Task.Run(() =>
            {
                using (callback)
                {
                    try
                    {
                        var httpClient = new HttpClient();
                        foreach (var header in headers.AllKeys)
                            if (header.ToLower() != "content-type")
                                httpClient.DefaultRequestHeaders.Add(header, headers.GetValues(header));

                        HttpResponseMessage response = null;
                        if (method == "OPTIONS")
                        {
                            // CORS のやつだし、テンプレでいいでしょ
                            StatusCode = 200;
                            MimeType = "text/plain";
                            Stream = null;
                            Headers.Set("Content-Length", "0");
                            Headers.Set("Content-Type", "text/plain; charset=utf-8");
                            Headers.Set("Access-Control-Allow-Headers", "Accept, Accept-Encoding, Content-Type, Authorization");
                            Headers.Set("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                            Headers.Set("Access-Control-Allow-Origin", "https://abema.tv");
                            Headers.Set("Access-Control-Max-Age", "86400");
                            Headers.Set("Vary", "Origin");
                            Headers.Set("Alt-Svc", "clear");
                            Headers.Set("Date", DateTime.Now.ToString("R"));
                            Headers.Set("Via", "1.1 google");
                            callback.Continue();
                            return;
                        }
                        else
                            CapturingRequest(url, headers, postData);

                        if (method == "GET")
                            response = httpClient.GetAsync(url).Result;
                        if (method == "POST")
                        {
                            var httpContent = new StringContent2(postData, Encoding.UTF8, "application/json");
                            response = httpClient.PostAsync(url, httpContent).Result;
                        }
                        if (method == "PUT")
                        {
                            var httpContent = new StringContent2(postData, Encoding.UTF8, "application/json");
                            response = httpClient.PutAsync(url, httpContent).Result;
                        }

                        Debug.WriteLine($"APICALL: {url}");
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

                        CapturingResponse(url, Headers, response.Content.ReadAsStringAsync().Result);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    finally
                    {
                        if (!callback.IsDisposed)
                            callback.Continue();
                    }
                }
            });

            return true;
        }

        #endregion

        private void CapturingRequest(string url, NameValueCollection headers, string body)
            => _networkHandler.OnHandlingRequest(new NetworkEventArgs(url, headers, body));

        private void CapturingResponse(string url, NameValueCollection headers, string body)
            => _networkHandler.OnHandlingResponse(new NetworkEventArgs(url, headers, body));
    }
}