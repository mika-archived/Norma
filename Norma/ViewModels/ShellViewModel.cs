using Norma.ViewModels.Controls;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ShellViewModel : ViewModel
    {
        public ShellViewModel()
        {
            Title = "Norma - AbemaTV";
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

        #region AbemaStatusViewModel

        private AbemaStatusViewModel _abemaStatusViewModel;

        public AbemaStatusViewModel StatusBar
        {
            get { return _abemaStatusViewModel; }
            set { SetProperty(ref _abemaStatusViewModel, value); }
        }

        #endregion
    }
}