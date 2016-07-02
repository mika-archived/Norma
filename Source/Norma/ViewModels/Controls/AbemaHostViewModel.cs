using CefSharp;
using CefSharp.Wpf;

using Newtonsoft.Json.Linq;

using Norma.Eta.Models;
using Norma.Eta.Models.Operations;
using Norma.Eta.Mvvm;
using Norma.Gamma.Models;
using Norma.Models;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AbemaHostViewModel : ViewModel, IOperationRequestAware, INetworkCaptureRequestAware
    {
        private readonly AbemaState _abemaState;
        private readonly Configuration _configuration;
        private readonly Reservation _reservation;
        private JavaScriptHost _javaScritHost;

        public AbemaHostViewModel(AbemaState abemaState, Configuration configuration, Connector connector,
                                  Reservation reservation,
                                  NetworkHandler networkHandler)
        {
            _abemaState = abemaState;
            _configuration = configuration;
            _reservation = reservation;
            connector.RegisterInsance<ChangeChannelOp>(this);
            networkHandler.RegisterInstance(this, e => e.Url.EndsWith("/slotReservations"));
            Address = $"https://abema.tv/now-on-air/{configuration.Root.LastViewedChannel.ToUrlString()}";
        }

        #region Implementation of INetworkCaptureRequestAware

        public void OnRequestHandling(NetworkEventArgs e)
        {
            dynamic json = JObject.Parse(e.Contents);
            var id = (string) json.slotReservations[0].slotId;
            _reservation.AddReservation(new Slot {Id = id});
        }

        #endregion

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
            _javaScritHost = new JavaScriptHost(WebBrowser, _configuration).AddTo(this);
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