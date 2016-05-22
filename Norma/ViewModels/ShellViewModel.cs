using System.Reactive.Linq;
using System.Windows.Input;

using Norma.Extensions;
using Norma.Interactivity;
using Norma.Models;
using Norma.ViewModels.Controls;
using Norma.ViewModels.Internal;
using Norma.Views;

using Prism.Commands;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ShellViewModel : ViewModel
    {
        private readonly Configuration _configuration;
        public AbemaHostViewModel HostViewModel { get; }
        public AbemaTVGuideViewModel TvGuideViewModel { get; }
        public AbemaStatusViewModel StatusBar { get; }
        public InteractionRequest2 TransitionRequest { get; }
        public InteractionRequest2 ModalTransitionRequest { get; }
        public ReadOnlyReactiveProperty<string> Title { get; private set; }
        public ReactiveProperty<bool> IsTopMost { get; private set; }

        public ShellViewModel(AbemaState abemaState, Configuration configuration, Models.Timetable timetable)
        {
            _configuration = configuration;

            HostViewModel = new AbemaHostViewModel(abemaState, configuration).AddTo(this);
            TvGuideViewModel = new AbemaTVGuideViewModel(this, configuration, timetable).AddTo(this);
            StatusBar = new AbemaStatusViewModel().AddTo(this);
            TransitionRequest = new InteractionRequest2();
            ModalTransitionRequest = new InteractionRequest2();

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
            _configuration.Save();
        }

        #endregion

        #region OpenTimetableCommand

        private ICommand _openTimetableCommand;

        public ICommand OpenTimetableCommand
            => _openTimetableCommand ?? (_openTimetableCommand = new DelegateCommand(OpenTimetable));

        private void OpenTimetable() => TransitionRequest.Raise(new WindowNotification(typeof(TimetableWindow)));

        #endregion

        #region OpenSettingsCommand

        private ICommand _openSettingsCommand;

        public ICommand OpenSettingsCommand
            => _openSettingsCommand ?? (_openSettingsCommand = new DelegateCommand(OpenSettings));

        private void OpenSettings() => ModalTransitionRequest.Raise(new WindowNotification(typeof(SettingsWindow)));

        #endregion
    }
}