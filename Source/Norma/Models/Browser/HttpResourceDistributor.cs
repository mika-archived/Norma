using System;
using System.Collections.Generic;
using System.Windows.Controls;

using HttpMonitorLib;

using Microsoft.Practices.ServiceLocation;

using Norma.Eta.Models;

namespace Norma.Models.Browser
{
    //Distributor
    internal class HttpResourceDistributor
    {
        private readonly Configuration _configuration;
        private readonly HttpMon _httpMonitor;
        private readonly List<IHttpResourceHandler> _resourceHandlers;

        public HttpResourceDistributor(WebBrowser webBrowser)
        {
            _configuration = ServiceLocator.Current.GetInstance<Configuration>();
            _httpMonitor = new HttpMon {IEWindow = webBrowser.Handle.ToInt32()};
            _resourceHandlers = new List<IHttpResourceHandler>();
            AttachEvents();
        }

        private void AttachEvents()
        {
            _httpMonitor.OnRequest += HttpMonitorOnRequest;
            _httpMonitor.OnResponse += HttpMonitorOnResponse;

            // https://api.abema.io
            _resourceHandlers.Add(new ApiResourceHandler());

            // https://media.abema.io
            if (_configuration.Root.Others.IsEnabledExperimentalFeatures)
                _resourceHandlers.Add(new VideoResourceHandler());
        }

        private void HttpMonitorOnResponse(int id, int containerId, string url, int responseCode, string headers)
        {
        }

        private void HttpMonitorOnRequest(int id, int containerId, string url, string headers, string method, object postData)
        {
        }
    }
}