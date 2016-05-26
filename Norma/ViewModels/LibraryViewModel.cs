using System.Diagnostics;
using System.Windows.Input;

using Norma.Eta.Mvvm;
using Norma.Models;

using Prism.Commands;

namespace Norma.ViewModels
{
    internal class LibraryViewModel : ViewModel
    {
        private readonly Library _library;

        public string Name => _library.Name;

        public string Url => _library.Url.Replace("https://", "").Replace("http://", "");

        public string License => _library.License;

        public LibraryViewModel(Library library)
        {
            _library = library;
        }

        #region OpenHyperlinkCommand

        private ICommand _openHyperlinkCommand;

        public ICommand OpenHyperlinkCommand =>
            _openHyperlinkCommand ?? (_openHyperlinkCommand = new DelegateCommand(OpenHyperlink));

        private void OpenHyperlink() => Process.Start(_library.Url);

        #endregion
    }
}