using CefSharp.Wpf;

using Norma.Extensions;
using Norma.Models;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AbemaHostViewModel : ViewModel
    {
        private readonly AbemaState _abemaState;
        private JavaScriptHost _javaScritHost;

        public AbemaHostViewModel(AbemaState abemaState, Configuration configuration)
        {
            _abemaState = abemaState;
            Address = $"https://abema.tv/now-on-air/{configuration.Root.LastViewedChannel.ToUrlString()}";
        }

        private void WebBrowserInitialized()
        {
            if (WebBrowser == null)
                return;
            _javaScritHost = new JavaScriptHost(WebBrowser, _abemaState).AddTo(this);
            _javaScritHost.Address = Address; // Initialize
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
            set
            {
                if (!SetProperty(ref _address, value) || _javaScritHost == null)
                    return;
                _javaScritHost.Address = value;
                _abemaState.OnChannelChanged(value);
            }
        }

        #endregion
    }
}