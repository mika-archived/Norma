using CefSharp;

namespace Norma.Models.Browser
{
    internal class HttpResourceHandlerFactory : IResourceHandlerFactory
    {
        #region Implementation of IResourceHandlerFactory

        public IResourceHandler GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame,
                                                   IRequest request)
        {
            // Handling request to "https://api.abema.io"
            return request.Url.StartsWith("https://api.abema.io") ? new HttpResourceHandler() : null;
        }

        public bool HasHandlers => true;

        #endregion
    }
}