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
        private readonly ConnectOps _connectOps;
        private readonly Notifier _notifier;

        public ShellViewModel(Timetable timetable, Reservation reservation, ConnectOps connectOps)
        {
            _connectOps = connectOps;
            _notifier = new Notifier(timetable, reservation).AddTo(this);
            _notifier.Start();
        }

        #region Overrides of ViewModel

        public override void Dispose()
        {
            base.Dispose();
            _connectOps.Dispose();
        }

        #endregion

        #region ExitCommand

        private ICommand _exitCommand;
        public ICommand ExitCommand => _exitCommand ?? (_exitCommand = new DelegateCommand(Exit));

        private void Exit() => Application.Current.Shutdown();

        #endregion
    }
}