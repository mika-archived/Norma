using System.Windows.Input;

using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace Norma.Iota.ViewModels.WindowContents
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class RsvDetailsContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        public InteractionRequest<Notification> WindowCloseRequest { get; }
        public string WindowTitle => Resources.RsvSuccess;

        public RsvDetailsContentViewModel()
        {
            WindowCloseRequest = new InteractionRequest<Notification>();
            ViewModelHelper.Subscribe(this, w => w.Notification, w =>
            {
                /*
                var model = RawNotification.Model as Reserve;
                if (model == null)
                    return;
                if (model is RsvProgram)
                    Reservation = new RsvProgramViewModel(model as RsvProgram);
                else if (model is RsvTime)
                    Reservation = new RsvTimeViewModel(model as RsvTime);
                else if (model is RsvKeyword)
                    Reservation = new RsvKeywordViewModel(model as RsvKeyword);
                else
                    throw new NotSupportedException();
                    */
            }).AddTo(this);
        }

        /*

        #region Reservation

        private ReservationViewModel _reservation;

        public ReservationViewModel Reservation
        {
            get { return _reservation; }
            set { SetProperty(ref _reservation, value); }
        }

        #endregion

        */

        #region OkCommand

        private ICommand _okCommand;
        public ICommand OkCommand => _okCommand ?? (_okCommand = new DelegateCommand(Ok));

        private void Ok() => WindowCloseRequest.Raise(null);

        #endregion
    }
}