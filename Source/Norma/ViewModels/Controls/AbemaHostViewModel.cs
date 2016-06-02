using CefSharp;
using CefSharp.Wpf;

using Norma.Eta.Models;
using Norma.Eta.Models.Operations;
using Norma.Eta.Mvvm;
using Norma.Models;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AbemaHostViewModel : ViewModel, IOperationRequestAware
    {
        private readonly AbemaState _abemaState;
        private readonly Configuration _configuration;
        private JavaScriptHost _javaScritHost;

        public AbemaHostViewModel(AbemaState abemaState, Configuration configuration, Connector connector)
        {
            _abemaState = abemaState;
            _configuration = configuration;
            connector.RegisterInsance<ChangeChannelOp>(this);
            Address = $"https://abema.tv/now-on-air/{configuration.Root.LastViewedChannel.ToUrlString()}";
        }

        #region Implementation of IOperationRequestAware

        public void Invoke(IOperation operation)
        {
            var args = operation as ChangeChannelOp;
            var channel = AbemaChannelExt.FromUrlString(args?.Context.ToString()).ToUrlString();
            Address = $"https://abema.tv/now-on-air/{channel}";
        }

        #endregion

        private void WebBrowserInitialized()
        {
            if (WebBrowser == null)
                return;
            _javaScritHost = new JavaScriptHost(WebBrowser, _abemaState, _configuration).AddTo(this);
            _javaScritHost.Address = Address; // Initialize
        }

        #region Overrides of ViewModel

        public override void Dispose()
        {
            base.Dispose();

            WebBrowser.Dispose();
            Cef.Shutdown();
        }

        #endregion

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