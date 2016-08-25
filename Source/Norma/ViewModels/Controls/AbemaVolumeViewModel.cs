using System;

using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Models;

using Reactive.Bindings;

namespace Norma.ViewModels.Controls
{
    internal class AbemaVolumeViewModel : ViewModel, INetworkCaptureResponseAware
    {
        private bool _isAttached;
        public ReactiveProperty<int> Volume { get; }

        public AbemaVolumeViewModel(Configuration configuration, NetworkHandler networkHandler)
        {
            Volume = ReactiveProperty.FromObject(configuration.Root.Internal, w => w.Volume).AddTo(this);
            Volume.Subscribe(w => VolumeManager.SetVolume(Volume.Value)).AddTo(this);

            networkHandler.RegisterInstance(this, w => w.Url.StartsWith("https://api.abema.io/v1/slotAudience"));
        }

        #region Implementation of INetworkCaptureResponseAware

        public void OnResponseHandling(NetworkEventArgs e)
        {
            if (_isAttached)
                return;
            _isAttached = true;
            VolumeManager.SetVolume(Volume.Value);
        }

        #endregion
    }
}