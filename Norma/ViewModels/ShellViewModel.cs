using System.Windows.Input;

using Norma.Interactivity;
using Norma.ViewModels.Controls;
using Norma.ViewModels.Internal;
using Norma.Views;

using Prism.Commands;

namespace Norma.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ShellViewModel : ViewModel
    {
        public AbemaHostViewModel HostViewModel { get; }
        public AbemaTVGuideViewModel TvGuideViewModel { get; }
        public AbemaStatusViewModel StatusBar { get; }
        public InteractionRequest2 TransitionRequest { get; }

        public ShellViewModel()
        {
            Title = "AbemaTV Start Page - Norma";
            HostViewModel = new AbemaHostViewModel(this);
            TvGuideViewModel = new AbemaTVGuideViewModel(this);
            StatusBar = new AbemaStatusViewModel();
            TransitionRequest = new InteractionRequest2();
        }

        #region OpenTimetableCommand

        private ICommand _openTimetableCommand;

        public ICommand OpenTImetableCommand
            => _openTimetableCommand ?? (_openTimetableCommand = new DelegateCommand(OpenTimetable));

        private void OpenTimetable()
        {
            TransitionRequest.Raise(typeof(TimetableWindow));
        }

        #endregion

        #region Title

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion
    }
}