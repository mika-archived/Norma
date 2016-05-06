using CefSharp.Wpf;

using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AbemaHostViewModel : ViewModel
    {
        public AbemaHostViewModel()
        {
            Address = "https://abema.tv/";
        }

        private void WebBrowserInitialized()
        {
            if (WebBrowser == null)
                return;
        }

        #region WebBrowser

        private IWpfWebBrowser _webBrowser;

        public IWpfWebBrowser WebBrowser
        {
            get { return _webBrowser; }
            set
            {
                if (SetProperty(ref _webBrowser, value))
                    WebBrowserInitialized();
            }
        }

        #endregion

        #region Address

        private string _address;

        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        #endregion
    }
}