using System;
using System.Threading;

using CefSharp;
using CefSharp.Wpf;

using Microsoft.Practices.ServiceLocation;

using Newtonsoft.Json.Linq;

using Norma.Delta.Services;
using Norma.Eta.Models;
using Norma.Eta.Models.Enums;
using Norma.Eta.Models.Operations;
using Norma.Eta.Mvvm;
using Norma.Models;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AbemaHostViewModel : ViewModel, IOperationRequestAware, INetworkCaptureRequestAware
    {
        private readonly AbemaState _abemaState;
        private readonly ReservationService _reservationService;
        private JavaScriptHost _javaScritHost;

        public AbemaHostViewModel()
        {
            _abemaState = ServiceLocator.Current.GetInstance<AbemaState>();
            _reservationService = ServiceLocator.Current.GetInstance<ReservationService>();

            var connector = ServiceLocator.Current.GetInstance<Connector>();
            connector.RegisterInsance<ChangeChannelOp>(this);

            var configuration = ServiceLocator.Current.GetInstance<Configuration>();
            var networkHandler = ServiceLocator.Current.GetInstance<NetworkHandler>();
            networkHandler.RegisterInstance(this, e => e.Url.EndsWith("/slotReservations"));
            Address = $"https://abema.tv/now-on-air/{configuration.Root.LastViewedChannelStr}";
        }

        #region Implementation of INetworkCaptureRequestAware

        public void OnRequestHandling(NetworkEventArgs e)
        {
            dynamic json = JObject.Parse(e.Contents);
            var id = (string) json.slotReservations[0].slotId;
            _reservationService.InsertSlotReservation2(id);
        }

        #endregion

        #region Implementation of IOperationRequestAware

        public void Invoke(IOperation operation)
        {
            var args = operation as ChangeChannelOp;
            var channel = AbemaChannelExt.ToIdentifier(args?.Context.ToString());
            // うーん？
            Address = "https://abema.tv";
            do
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
            }
            while (_javaScritHost == null);
            Address = $"https://abema.tv/now-on-air/{channel}";
        }

        #endregion

        private void WebBrowserInitialized()
        {
            if (WebBrowser == null)
                return;
            _javaScritHost = new JavaScriptHost(WebBrowser);
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
                _abemaState.OnChannelChanged(value);
            }
        }

        #endregion
    }
}