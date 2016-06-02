using System.Windows;
using System.Windows.Input;

using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Ipsilon.Models;

using Prism.Commands;

namespace Norma.Ipsilon.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ShellViewModel : ViewModel
    {
        private readonly Notifier _notifier;

        public ShellViewModel(Configuration configuration, Timetable timetable, Reservation reservation)
        {
            _notifier = new Notifier(configuration, timetable, reservation).AddTo(this);
            _notifier.Start();
        }

        #region ExitCommand

        private ICommand _exitCommand;
        public ICommand ExitCommand => _exitCommand ?? (_exitCommand = new DelegateCommand(Exit));

        private void Exit() => Application.Current.Shutdown();

        #endregion
    }
}