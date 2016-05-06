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

        #region WebBrowser

        private IWpfWebBrowser _webBrowser;

        public IWpfWebBrowser WebBrowser
        {
            get { return _webBrowser; }
            set { SetProperty(ref _webBrowser, value); }
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