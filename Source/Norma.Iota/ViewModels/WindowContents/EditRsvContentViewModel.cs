using System;

using Norma.Eta.Models;
using Norma.Eta.Models.Reservations;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;
using Norma.Iota.ViewModels.Controls;

namespace Norma.Iota.ViewModels.WindowContents
{
    internal class EditRsvContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        public string WindowTitle => Resources.EditRsv;

        public EditRsvContentViewModel(Reservation rsv)
        {
            ViewModelHelper.Subscribe(this, nameof(Notification), w =>
            {
                var model = RawNotification.Model as RsvAll;
                if (model == null)
                    return;
                InteractionViewModel<DataPassingNotification> vm;
                if (model.Type == nameof(RsvKeyword))
                {
                    vm = new KeywordRsvControlViewModel(rsv)
                    {
                        Notification = new DataPassingNotification {Model = model.Cast<RsvKeyword>()}
                    };
                }
                else if (model.Type == nameof(RsvTime))
                {
                    vm = new TimeRsvControlViewModel(rsv)
                    {
                        Notification = new DataPassingNotification {Model = model.Cast<RsvTime>()}
                    };
                }
                else
                    throw new NotSupportedException();
                TargetViewModel = vm;
            });
        }

        #region TargetViewModel

        private ViewModel _targetViewModel;

        public ViewModel TargetViewModel
        {
            get { return _targetViewModel; }
            set { SetProperty(ref _targetViewModel, value); }
        }

        #endregion
    }
}