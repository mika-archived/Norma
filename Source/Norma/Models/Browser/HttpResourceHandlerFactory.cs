using CefSharp;

namespace Norma.Models.Browser
{
    internal class HttpResourceHandlerFactory : IResourceHandlerFactory
    {
        #region Implementation of IResourceHandlerFactory

        public IResourceHandler GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame,
                                                   IRequest request)
        {
            var url = request.Url;
            if (url.StartsWith("https://api.abema.io"))
                return new ApiResourceHandler();
            if (url.StartsWith("https://media.abema.io"))
                return new VideoResourceHandler();
            return null;
        }

        public bool HasHandlers => true;

        #endregion
    }
}