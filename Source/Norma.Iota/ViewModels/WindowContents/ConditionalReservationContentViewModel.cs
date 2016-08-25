using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;
using Norma.Iota.Models;
using Norma.Iota.ViewModels.Contents;

namespace Norma.Iota.ViewModels.WindowContents
{
    internal class ConditionalReservationContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        public string WindowTitle => Resources.ConditionalReservation;

        public ConditionalReservationContentViewModel()
        {
            FinishInteraction += () =>
            {
                KeywordReservationContentViewModel.Dispose();
                TimeReservationContentViewModel.Dispose();
            };
            ViewModelHelper.Subscribe(this, w => w.Notification, w => Reset());
        }

        private void Reset()
        {
            // (ヽ´ω`)...
            var item = (ReservationItem) RawNotification.Model;
            KeywordReservationContentViewModel = new KeywordReservationContentViewModel(this, item);
            TimeReservationContentViewModel = new TimeReservationContentViewModel(this, item);

            // なんかなー
            SelectedIndex = 0;
            if (item == null)
                return;
            if (item.Type == Resources.Keyword)
                SelectedIndex = 0;
            else if (item.Type == Resources.Time)
                SelectedIndex = 1;
            else if (item.Type == Resources.Query)
                SelectedIndex = 2;
        }

        #region SelectedIndex

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        #endregion

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