using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;
using Norma.Iota.Models;
using Norma.Iota.ViewModels.Controls;

namespace Norma.Iota.ViewModels.WindowContents
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class DetailsRsvContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        public TimeRsvControlViewModel TimeRsvControlViewModel { get; }
        public KeywordRsvControlViewModel KeywordRsvControlViewModel { get; }

        public DetailsRsvContentViewModel(Reservation reservation)
        {
            TimeRsvControlViewModel = new TimeRsvControlViewModel(reservation, false);
            KeywordRsvControlViewModel = new KeywordRsvControlViewModel(reservation, false);
            ViewModelHelper.Subscribe(this, nameof(Notification), w =>
            {
                var model = RawNotification.Model as WrapSlot;
                if (model == null)
                    return;
                WindowTitle = $"{model.Model.Title} - {Resources.ViewingDRsv}";
                TimeRsvControlViewModel.Notification = RawNotification;
                KeywordRsvControlViewModel.Notification = RawNotification;
            }).AddTo(this);
        }

        #region WindowTitle

        private string _windowTitle;

        public string WindowTitle
        {
            get { return _windowTitle; }
            set { SetProperty(ref _windowTitle, value); }
        }

        #endregion
    }
}