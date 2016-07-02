using System.Diagnostics;

using CefSharp;

namespace Norma.Models.Browser
{
    // Implementation
    // https://github.com/cefsharp/CefSharp/blob/master/CefSharp.Example/RequestHandler.cs
    internal class AbemaHttpRequestHandler : IRequestHandler
    {
        #region Implementation of IRequestHandler

        public bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
                                   bool isRedirect)
        {
            return false;
        }

        public bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame,
                                     string targetUrl,
                                     WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        public bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode,
                                       string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            return false;
        }

        public void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
            // TODO: Add your own code here for handling scenarios where a plugin crashed, for one reason or another.
        }

        public CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame,
                                                   IRequest request,
                                                   IRequestCallback callback)
        {
            return CefReturnValue.Continue;
        }

        public bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy,
                                       string host, int port,
                                       string realm, string scheme, IAuthCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.

            callback.Dispose();
            return false;
        }

        public void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        {

        }

        public bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize,
                                   IRequestCallback callback)
        {
            return false;
        }

        public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
                                       ref string newUrl) {}

        public bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            return false;
        }

        public void OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        {

        }

        public bool OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
                                       IResponse response)
        {
            Debug.WriteLine($"[Chromium Request] {request.Url}");
            return false;
        }

        public IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame,
                                                         IRequest request,
                                                         IResponse response)
        {
            return null;
        }

        public void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
                                           IResponse response, UrlRequestStatus status, long receivedContentLength) {}

        #endregion
    }
}