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
            ViewModelHelper.Subscribe(this, w => w.Notification, w => Reset());
        }

        private void Reset()
        {
            // (ヽ´ω`)...
            KeywordReservationContentViewModel = new KeywordReservationContentViewModel(this);
        }

        #region KeywordReservationContentViewModel

        private KeywordReservationContentViewModel _keywordReservationContentViewModel;

        public KeywordReservationContentViewModel KeywordReservationContentViewModel
        {
            get { return _keywordReservationContentViewModel; }
            set { SetProperty(ref _keywordReservationContentViewModel, value); }
        }

        #endregion
    }
}