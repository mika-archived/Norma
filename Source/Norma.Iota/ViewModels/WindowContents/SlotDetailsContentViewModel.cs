using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Norma.Eta.Models;
using Norma.Eta.Models.Reservations;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;
using Norma.Iota.Models;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace Norma.Iota.ViewModels.WindowContents
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class SlotDetailsContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        private readonly Reservation _rsvs;
        public InteractionRequest<DataPassingNotification> ResponseRequest { get; }
        public InteractionRequest<DataPassingNotification> DetailsRsvRequest { get; }
        public ObservableCollection<string> Cast { get; }
        public ObservableCollection<string> Staff { get; }

        public SlotDetailsContentViewModel(Reservation reservation)
        {
            _rsvs = reservation;
            ResponseRequest = new InteractionRequest<DataPassingNotification>();
            DetailsRsvRequest = new InteractionRequest<DataPassingNotification>();
            Cast = new ObservableCollection<string>();
            Staff = new ObservableCollection<string>();
            ViewModelHelper.Subscribe(this, w => w.Notification, w =>
            {
                var model = RawNotification.Model as WrapSlot;
                Cast.Clear();
                Staff.Clear();
                if (model == null)
                    return;
                WindowTitle = $"{model.Model.Title} - {Resources.ProgramDetails} - Norma";
                Title = model.Model.Title;
                Date = model.StartAt.ToString("MM/DD");
                Time = $"{model.Model.StartAt.ToString("MM/dd HH:mm")} ～ {model.Model.EndAt.ToString("MM/dd HH:mm")}";
                Description = model.DetailHighlight;
                model.Cast?.ForEach(x => Cast.Add(x));
                model.Staff?.ForEach(x => Staff.Add(x));
                Thumbnail = $"https://hayabusa.io/abema/programs/{model.Model.DisplayProgramId}/thumb001.w200.h112.jpg";
                Channel = AbemaChannelExt.ToLocaleString(model.Model.ChannelId);
                ((DelegateCommand) AddReservationCommand).RaiseCanExecuteChanged();
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

        #region Title

        private string _title;

        public string Title
        {
            get { return _title; }
            private set { SetProperty(ref _title, value); }
        }

        #endregion

        #region Date

        private string _date;

        public string Date
        {
            get { return _date; }
            private set { SetProperty(ref _date, value); }
        }

        #endregion

        #region Time

        private string _time;

        public string Time
        {
            get { return _time; }
            private set { SetProperty(ref _time, value); }
        }

        #endregion

        #region Description

        private string _description;

        public string Description
        {
            get { return _description; }
            private set { SetProperty(ref _description, value); }
        }

        #endregion

        #region Thumbnail

        private string _thumbnail;

        public string Thumbnail
        {
            get { return _thumbnail; }
            set { SetProperty(ref _thumbnail, value); }
        }

        #endregion

        #region Channel

        private string _channel;

        public string Channel
        {
            get { return _channel; }
            set { SetProperty(ref _channel, value); }
        }

        #endregion

        #region AddNormalReservationCommand

        private ICommand _addReservationCommand;

        public ICommand AddReservationCommand
            => _addReservationCommand ?? (_addReservationCommand = new DelegateCommand(AddReservation, CanAddRsv));

        private void AddReservation()
        {
            _rsvs.AddReservation(((WrapSlot) RawNotification.Model).Model);

            var slot = ((WrapSlot) RawNotification.Model).Model;
            ResponseRequest.Raise(new DataPassingNotification
            {
                Model = new RsvProgram {ProgramId = slot.Id, StartDate = slot.StartAt}
            });
            ((DelegateCommand) AddReservationCommand).RaiseCanExecuteChanged();
        }

        private bool CanAddRsv()
        {
            if (RawNotification == null)
                return false;
            var model = (WrapSlot) RawNotification.Model;
            return !_rsvs.RsvsByProgram.Any(w => w.IsEnable && w.ProgramId == model.Model.Id) &&
                   model.CanRsv;
        }

        #endregion

        #region AddSeriesReservationCommand

        private ICommand _addSeriesRsvCommand;

        public ICommand AddSeriesRsvCommand
            => _addSeriesRsvCommand ?? (_addSeriesRsvCommand = new DelegateCommand(AddSeriesRsv, CanAddSeriesRsv));

        private void AddSeriesRsv()
        {
            var slot = ((WrapSlot) RawNotification.Model).Model;
            _rsvs.AddReservation(slot.Programs[0].Series.Id);
            ((DelegateCommand) AddReservationCommand).RaiseCanExecuteChanged();
        }

        private bool CanAddSeriesRsv()
        {
            if (RawNotification == null)
                return false;
            var model = (WrapSlot) RawNotification.Model;
            return !_rsvs.RsvBySeries.Any(w => w.IsEnable && w.SeriesId == model.Model.Programs[0].Series.Id) &&
                   model.CanRsv;
        }

        #endregion

        #region AddDetailsReservationCommand

        private ICommand _addDetailsRsvCommand;

        public ICommand AddDetailsRsvCommand
            => _addDetailsRsvCommand ?? (_addDetailsRsvCommand = new DelegateCommand(AddDetailsRsv));

        private void AddDetailsRsv()
            => DetailsRsvRequest.Raise(new DataPassingNotification {Model = RawNotification.Model});

        #endregion
    }
}