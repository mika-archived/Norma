using System.Reactive.Linq;

using Norma.Eta.Mvvm;
using Norma.Eta.Services;
using Norma.Models;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels.Controls
{
    internal class AbemaProgramInfoViewModel : ViewModel
    {
        public ReadOnlyReactiveProperty<string> Title { get; private set; }
        public ReadOnlyReactiveProperty<string> Description { get; private set; }
        public ReadOnlyReactiveProperty<bool> HasInfo { get; private set; }
        public ReadOnlyReactiveProperty<string> Thumbnail1 { get; private set; }
        public ReadOnlyReactiveProperty<string> Thumbnail2 { get; private set; }
        public ReadOnlyReactiveProperty<string> AtChannel { get; private set; }
        public ReadOnlyReactiveProperty<string> Range { get; private set; }
        public ReadOnlyReactiveCollection<string> Casts { get; }
        public ReadOnlyReactiveCollection<string> Crews { get; private set; }

        public AbemaProgramInfoViewModel(AbemaState abemaState, StatusService statusService)
        {
            var programHost = new ProgramHost(abemaState, statusService).AddTo(this);
            Title = programHost.ObserveProperty(w => w.Title).ToReadOnlyReactiveProperty().AddTo(this);
            Description = programHost.ObserveProperty(w => w.Description).ToReadOnlyReactiveProperty().AddTo(this);
            HasInfo = programHost.ObserveProperty(w => w.Title)
                                 .Select(w => !string.IsNullOrWhiteSpace(w))
                                 .ToReadOnlyReactiveProperty().AddTo(this);
            Thumbnail1 = programHost.ObserveProperty(w => w.Thumbnail1).ToReadOnlyReactiveProperty().AddTo(this);
            Thumbnail2 = programHost.ObserveProperty(w => w.Thumbnail2).ToReadOnlyReactiveProperty().AddTo(this);
            AtChannel = abemaState.ObserveProperty(w => w.CurrentChannel)
                                  .Where(w => w != null)
                                  .Select(w => $"at {w.Name.Replace("チャンネル", "")}")
                                  .ToReadOnlyReactiveProperty().AddTo(this);
            Range = abemaState.ObserveProperty(w => w.CurrentSlot)
                              .Where(w => w != null)
                              .Select(w => $"{w.StartAt.ToString("t")} ～ {w.EndAt.ToString("t")}")
                              .ToReadOnlyReactiveProperty().AddTo(this);
            Casts = programHost.Casts.ToReadOnlyReactiveCollection().AddTo(this);
            Crews = programHost.Crews.ToReadOnlyReactiveCollection().AddTo(this);
        }
    }
}