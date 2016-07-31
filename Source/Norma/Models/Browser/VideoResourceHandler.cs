using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

using CefSharp;

using Microsoft.Practices.ServiceLocation;

using Norma.Eta.Models;
using Norma.Eta.Models.Configurations;
using Norma.Eta.Models.Enums;
using Norma.Gamma;

namespace Norma.Models.Browser
{
    // 強制画質変更
    internal class VideoResourceHandler : ResourceHandler
    {
        private readonly OperationConfig _config;
        private readonly Regex _pattern = new Regex(@"[0-9]{3,4}");

        public VideoResourceHandler()
        {
            _config = ServiceLocator.Current.GetInstance<Configuration>().Root.Operation;
        }

        public override bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            try
            {
                var httpClient = new HttpClient();
                foreach (var header in request.Headers.AllKeys)
                    if (header.ToLower() != "content-type")
                        httpClient.DefaultRequestHeaders.Add(header, request.Headers.GetValues(header));

                var url = request.Url;
                if (_config.VideoQuality != VideoQuality.Auto)
                    url = _pattern.Replace(url, _config.VideoQuality.ToProgressive(), 1);
                HttpResponseMessage response = null;
                if (request.Method == "OPTIONS")
                    response = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Options, url)).Result;

                if (request.Method == "GET")
                    response = httpClient.GetAsync(url).Result;
                if (request.Method == "POST")
                {
                    var content = request.PostData?.Elements.FirstOrDefault()?.GetBody();
                    var httpContent = new StringContent2(content, Encoding.UTF8, "application/json");
                    response = httpClient.PostAsync(url, httpContent).Result;
                }
                if (request.Method == "PUT")
                {
                    var content = request.PostData?.Elements.FirstOrDefault()?.GetBody();
                    var httpContent = new StringContent2(content, Encoding.UTF8, "application/json");
                    response = httpClient.PutAsync(url, httpContent).Result;
                }

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

                callback.Continue();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return true;
        }
    }
}