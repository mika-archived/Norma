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
        public AbemaHostViewModel HostViewModel { get; }
        public AbemaTVGuideViewModel TvGuideViewModel { get; }
        public AbemaStatusViewModel StatusBar { get; }
        public InteractionRequest2 TransitionRequest { get; }
        public InteractionRequest2 ModalTransitionRequest { get; }
        public ReadOnlyReactiveProperty<string> Title { get; private set; }

        public ShellViewModel(AbemaState abemaState)
        {
            HostViewModel = new AbemaHostViewModel(abemaState).AddTo(this);
            TvGuideViewModel = new AbemaTVGuideViewModel(this).AddTo(this);
            StatusBar = new AbemaStatusViewModel().AddTo(this);
            TransitionRequest = new InteractionRequest2();
            ModalTransitionRequest = new InteractionRequest2();

            Title = abemaState.ObserveProperty(w => w.CurrentSlot)
                              .Select(w => $"{w.Title} - Norma")
                              .ToReadOnlyReactiveProperty($"{abemaState.CurrentSlot.Title} - Norma")
                              .AddTo(this);
        }

        #region OpenTimetableCommand

        private ICommand _openTimetableCommand;

        public ICommand OpenTimetableCommand
            => _openTimetableCommand ?? (_openTimetableCommand = new DelegateCommand(OpenTimetable));

        private void OpenTimetable() => TransitionRequest.Raise(typeof(TimetableWindow));

        #endregion

        #region OpenSettingsCommand

        private ICommand _openSettingsCommand;

        public ICommand OpenSettingsCommand
            => _openSettingsCommand ?? (_openSettingsCommand = new DelegateCommand(OpenSettings));

        private void OpenSettings() => ModalTransitionRequest.Raise(typeof(SettingsWindow));

        #endregion
    }
}