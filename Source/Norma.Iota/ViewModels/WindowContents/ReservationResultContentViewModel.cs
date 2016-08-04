using System.Windows.Input;

using Norma.Delta.Models;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace Norma.Iota.ViewModels.WindowContents
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ReservationResultContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        public InteractionRequest<Notification> WindowCloseRequest { get; }
        public string WindowTitle => Resources.RsvSuccess;

        public ReservationResultContentViewModel()
        {
            WindowCloseRequest = new InteractionRequest<Notification>();
            ViewModelHelper.Subscribe(this, w => w.Notification, w =>
            {
                var model = RawNotification.Model;
                if (model is Slot) // 単体予約
                    Message = string.Format(Resources.SlotReservationResult, ((Slot) model).Title);
                else if (model is Series) // シリーズ予約
                    Message = string.Format(Resources.SeriesReservationResult, ((Series) model).SeriesId);
            }).AddTo(this);
        }

        #region Message

        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        #endregion

        #region OkCommand

        private ICommand _okCommand;
        public ICommand OkCommand => _okCommand ?? (_okCommand = new DelegateCommand(Ok));

        private void Ok() => WindowCloseRequest.Raise(null);

        #endregion
    }
}