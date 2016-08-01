using CefSharp;

using Microsoft.Practices.ServiceLocation;

using Norma.Eta.Models;

namespace Norma.Models.Browser
{
    internal class HttpResourceHandlerFactory : IResourceHandlerFactory
    {
        private readonly Configuration _configuration;

        public HttpResourceHandlerFactory()
        {
            _configuration = ServiceLocator.Current.GetInstance<Configuration>();
        }

        #region Implementation of IResourceHandlerFactory

        public IResourceHandler GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame,
                                                   IRequest request)
        {
            var url = request.Url;
            if (url.StartsWith("https://api.abema.io"))
                return new ApiResourceHandler();
            if (url.StartsWith("https://media.abema.io") && _configuration.Root.Others.IsEnabledExperimentalFeatures)
                return new VideoResourceHandler();
            return null;
        }

        public bool HasHandlers => true;

        #endregion
    }
}