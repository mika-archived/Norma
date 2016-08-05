using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;
using Norma.Iota.ViewModels.Contents;

using Prism.Interactivity.InteractionRequest;

namespace Norma.Iota.ViewModels.WindowContents
{
    internal class ConditionalReservationContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        public InteractionRequest<Notification> WindowCloseRequest { get; }
        public string WindowTitle => Resources.ConditionalReservation;

        public ConditionalReservationContentViewModel()
        {
            WindowCloseRequest = new InteractionRequest<Notification>();
            WindowCloseRequest.Raised += (sender, e) =>
            {
                KeywordReservationContentViewModel.Dispose();
                TimeReservationContentViewModel.Dispose();
            };
            ViewModelHelper.Subscribe(this, w => w.Notification, w => Reset());
        }

        private void Reset()
        {
            // (ヽ´ω`)...
            KeywordReservationContentViewModel = new KeywordReservationContentViewModel(this);
            TimeReservationContentViewModel = new TimeReservationContentViewModel(this);
        }

        #region KeywordReservationContentViewModel

        private KeywordReservationContentViewModel _keywordReservationContentViewModel;

        public KeywordReservationContentViewModel KeywordReservationContentViewModel
        {
            get { return _keywordReservationContentViewModel; }
            set { SetProperty(ref _keywordReservationContentViewModel, value); }
        }

        #endregion

        #region TimeReservationContentViewModel

        private TimeReservationContentViewModel _timeReservationContentViewModel;

        public TimeReservationContentViewModel TimeReservationContentViewModel
        {
            get { return _timeReservationContentViewModel; }
            set { SetProperty(ref _timeReservationContentViewModel, value); }
        }

        #endregion
    }
}