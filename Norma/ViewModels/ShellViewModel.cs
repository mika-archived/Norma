using Norma.ViewModels.Controls;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ShellViewModel : ViewModel
    {
        public AbemaHostViewModel HostViewModel { get; }
        public AbemaTVGuideViewModel TvGuideViewModel { get; }
        public AbemaStatusViewModel StatusBar { get; }

        public ShellViewModel()
        {
            Title = "AbemaTV Start Page - Norma";
            HostViewModel = new AbemaHostViewModel(this);
            TvGuideViewModel = new AbemaTVGuideViewModel(this);
            StatusBar = new AbemaStatusViewModel();
        }

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