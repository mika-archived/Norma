using System.Collections.ObjectModel;

using Norma.Delta.Services;
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
        private readonly ReservationService _reservationService;
        private WrapSlot _model;
        private bool[] _reserved;
        public InteractionRequest<DataPassingNotification> ResponseRequest { get; }
        public InteractionRequest<DataPassingNotification> DetailsRsvRequest { get; }
        public ObservableCollection<string> Cast { get; }
        public ObservableCollection<string> Staff { get; }

        public SlotDetailsContentViewModel(ReservationService reservationService)
        {
            _reservationService = reservationService;
            ResponseRequest = new InteractionRequest<DataPassingNotification>();
            DetailsRsvRequest = new InteractionRequest<DataPassingNotification>();
            Cast = new ObservableCollection<string>();
            Staff = new ObservableCollection<string>();
            _reserved = new[] {false, false};
            ViewModelHelper.Subscribe(this, w => w.Notification, w =>
            {
                _model = RawNotification.Model as WrapSlot;
                if (_model == null)
                    return;
                _reserved = new[] {false, false};
                Cast.Clear();
                Staff.Clear();
                _model.RequestDetails();
                WindowTitle = $"{_model.Title} - {Resources.ProgramDetails} - Norma";
                Title = _model.Title;
                Date = _model.FixedStartAt.ToString("MM/DD");
                Time = $"{_model.StartAt.ToString("MM/dd HH:mm")} ～ {_model.EndAt.ToString("MM/dd HH:mm")}";
                Description = _model.Description;
                _model.Casts.ForEach(x => Cast.Add(x));
                _model.Crews.ForEach(x => Staff.Add(x));
                Thumbnail = $"https://hayabusa.io/abema/programs/{_model.ProgramId}/thumb001.w200.h112.jpg";
                Channel = _model.Channel.Name;
                RegisterSlotReservationCommand.RaiseCanExecuteChanged();
                RegisterSeriesReservationCommand.RaiseCanExecuteChanged();
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

        #region RegisterSlotReservationCommand

        private DelegateCommand _registerSlotReservationCommand;

        public DelegateCommand RegisterSlotReservationCommand
            => _registerSlotReservationCommand ??
               (_registerSlotReservationCommand = new DelegateCommand(RegisterSlotReservation, CanRegisterSlotReservation));

        private void RegisterSlotReservation()
        {
            _reservationService.InsertSlotReservation(_model.Slot);
            _reserved[0] = true;
            ResponseRequest.Raise(new DataPassingNotification {Model = _model.Slot});
            RegisterSlotReservationCommand.RaiseCanExecuteChanged();
        }

        private bool CanRegisterSlotReservation() => (_model?.CanSlotReservation ?? false) && !_reserved[0];

        #endregion

        #region RegisterSeriesReservationCommand

        private DelegateCommand _registerSeriesReservationCommand;

        public DelegateCommand RegisterSeriesReservationCommand
            => _registerSeriesReservationCommand ??
               (_registerSeriesReservationCommand = new DelegateCommand(RegisterSeriesReservation, CanRegisterSeriesReservation));

        private void RegisterSeriesReservation()
        {
            _reservationService.InsertSeriesReservation(_model.Series);
            _reserved[1] = true;
            ResponseRequest.Raise(new DataPassingNotification {Model = _model.Series});
            RegisterSeriesReservationCommand.RaiseCanExecuteChanged();
        }

        private bool CanRegisterSeriesReservation() => (_model?.CanSeriesReservation ?? false) && !_reserved[1];

        #endregion
    }
}