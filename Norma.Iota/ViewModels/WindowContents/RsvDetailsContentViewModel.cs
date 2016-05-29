using System;
using System.Windows.Input;

using Norma.Eta.Models.Reservations;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;
using Norma.Iota.ViewModels.Reservations;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace Norma.Iota.ViewModels.WindowContents
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class RsvDetailsContentViewModel : ViewModel, IInteractionRequestAware
    {
        public InteractionRequest<Notification> WindowCloseRequest { get; }
        public string WindowTitle => Resources.RsvSuccess;

        public RsvDetailsContentViewModel()
        {
            WindowCloseRequest = new InteractionRequest<Notification>();
            ViewModelHelper.Subscribe(this, nameof(Notification), w =>
            {
                var model = _notification.Model as Reserve;
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
            }).AddTo(this);
        }

        #region Reservation

        private ReservationViewModel _reservation;

        public ReservationViewModel Reservation
        {
            get { return _reservation; }
            set { SetProperty(ref _reservation, value); }
        }

        #endregion

        #region OkCommand

        private ICommand _okCommand;
        public ICommand OkCommand => _okCommand ?? (_okCommand = new DelegateCommand(Ok));

        private void Ok() => WindowCloseRequest.Raise(null);

        #endregion

        #region Implementation of IInteractionRequestAware

        #region Notification

        private DataPassingNotification _notification;

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                var notification = value as DataPassingNotification;
                if (notification == null)
                    return;
                _notification = notification;
                OnPropertyChanged();
            }
        }

        #endregion

        public Action FinishInteraction { get; set; }

        #endregion
    }
}