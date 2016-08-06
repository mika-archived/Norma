using System;

using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Models;

using Reactive.Bindings;

namespace Norma.ViewModels.Controls
{
    internal class AbemaVolumeViewModel : ViewModel
    {
        public ReactiveProperty<int> Volume { get; }

        public AbemaVolumeViewModel(Configuration configuration)
        {
            Volume = ReactiveProperty.FromObject(configuration.Root.Internal, w => w.Volume).AddTo(this);
            Volume.Subscribe(w => VolumeManager.SetVolume(Volume.Value)).AddTo(this);
        }
    }
}