using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Input;

using Norma.Eta;
using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Models;
using Norma.ViewModels.Controls;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ShellViewModel : ViewModel
    {
        private readonly Configuration _configuration;
        private readonly Timetable _timetable;
        public AbemaHostViewModel HostViewModel { get; }
        public AbemaTVGuideViewModel TvGuideViewModel { get; }
        public AbemaStatusViewModel StatusBar { get; }
        public InteractionRequest<INotification> SettingsRequest { get; }
        public ReadOnlyReactiveProperty<string> Title { get; private set; }
        public ReactiveProperty<bool> IsTopMost { get; private set; }

        public ShellViewModel(AbemaState abemaState, Configuration configuration, Timetable timetable)
        {
            _configuration = configuration;
            _timetable = timetable;

            HostViewModel = new AbemaHostViewModel(abemaState, configuration).AddTo(this);
            TvGuideViewModel = new AbemaTVGuideViewModel(this, configuration, timetable).AddTo(this);
            StatusBar = new AbemaStatusViewModel().AddTo(this);
            SettingsRequest = new InteractionRequest<INotification>();

            Title = abemaState.ObserveProperty(w => w.CurrentSlot)
                              .Select(w => $"{w?.Title ?? "AbemaTV"} - Norma")
                              .ToReadOnlyReactiveProperty($"{abemaState.CurrentSlot?.Title ?? "AbemaTV"} - Norma")
                              .AddTo(this);
            IsTopMost = ReactiveProperty.FromObject(configuration.Root.Internal, w => w.IsTopMost).AddTo(this);
        }

        #region Overrides of ViewModel

        public override void Dispose()
        {
            base.Dispose();
            // ?
            _timetable.Save();
            _configuration.Save();
        }

        #endregion

        #region OpenTimetableCommand

        private ICommand _openTimetableCommand;

        public ICommand OpenTimetableCommand
            => _openTimetableCommand ?? (_openTimetableCommand = new DelegateCommand(OpenTimetable));

        private void OpenTimetable()
        {
            if (File.Exists(NormaConstants.IotaExecutableFile))
                Process.Start(NormaConstants.IotaExecutableFile);
        }

        #endregion

        #region OpenSettingsCommand

        private ICommand _openSettingsCommand;

        public ICommand OpenSettingsCommand
            => _openSettingsCommand ?? (_openSettingsCommand = new DelegateCommand(OpenSettings));

        private void OpenSettings() => SettingsRequest.Raise(new Notification {Content = "Blank", Title = "Blank"});

        #endregion
    }
}