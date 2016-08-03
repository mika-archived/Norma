using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

using Norma.Delta.Models;
using Norma.Delta.Services;
using Norma.Eta.Extensions;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Iota.ViewModels.Controls;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace Norma.Iota.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ShellViewModel : ViewModel
    {
        private readonly DatabaseService _databaseService;
        private int _index; // 日付管理用(0 = 今日, 6 = 一週間後みたいな)
        public ObservableCollection<ChannelCellViewModel> Channels { get; }
        public List<string> AvailableDates { get; }
        public InteractionRequest<INotification> ProgramDetailsRequest { get; }
        public InteractionRequest<INotification> ReservationListRequest { get; }
        public InteractionRequest<DataPassingNotification> DetailReserveRequest { get; }

        public ShellViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            ProgramDetailsRequest = new InteractionRequest<INotification>();
            ReservationListRequest = new InteractionRequest<INotification>();
            DetailReserveRequest = new InteractionRequest<DataPassingNotification>();

            // TODO: Move to Model
            using (var connection = _databaseService.Connect())
            {
                var lastSyncTime = DateTime.Parse(connection.Metadata.Single(w => w.Key == Metadata.LastSyncTimeKey).Value);
                _index = (DateTime.Now - lastSyncTime).Days;
                AvailableDates = new List<string>();
                Channels = new ObservableCollection<ChannelCellViewModel>();
                for (var i = 0; i < 6; i++)
                    AvailableDates.Add(lastSyncTime.AddDays(i).ToString("MM/dd"));

                SelectedDate = AvailableDates[0];
            }
            //UpdateChannels();
        }

        // TODO: Move to Model
        private void UpdateChannels()
        {
            IsLoading = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Channels.Clear();
                CompositeDisposable.Dispose();
            });
            var list = new List<ChannelCellViewModel>();
            using (var connection = _databaseService.Connect())
            {
                var channels = connection.Channels.ToList();
                foreach (var channel in channels)
                {
                    var targetDate = DateTime.Parse(AvailableDates[_index]);
                    var targetMaxDate = targetDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                    var slots = connection.Slots.Where(w => w.Channel.ChannelId == channel.ChannelId)
                                          .Where(w => w.StartAt >= targetDate && w.StartAt <= targetMaxDate).ToList();
                    var vm = new ChannelCellViewModel(channel, slots, targetDate).AddTo(this);
                    list.Add(vm);
                }
            }
            foreach (var vm in list)
                Application.Current.Dispatcher.Invoke(() => Channels.Add(vm));
            IsLoading = false;
        }

        #region SelectedDate

        private string _selectedDate;

        public string SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (!SetProperty(ref _selectedDate, value))
                    return;
                _index = AvailableDates.IndexOf(_selectedDate);
                IsLoading = true;
                Observable.Return(0).Delay(TimeSpanExt.OneSecond).Subscribe(w => UpdateChannels());
            }
        }

        #endregion

        #region IsLoading

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        #endregion

        #region ReserveUsingKeywordOrTimeCommand

        private ICommand _rsvUsingKwdOrTimeCommand;

        public ICommand ReserveUsingKeywordOrTimeCommand
            => _rsvUsingKwdOrTimeCommand ?? (_rsvUsingKwdOrTimeCommand = new DelegateCommand(ReserveUsingKwdOrTime));

        private void ReserveUsingKwdOrTime() => DetailReserveRequest.Raise(new DataPassingNotification());

        #endregion

        #region OpenReservationListCommand

        private ICommand _openRsvListCommand;

        public ICommand OpenRsvListCommand
            => _openRsvListCommand ?? (_openRsvListCommand = new DelegateCommand(OpenRsvList));

        private void OpenRsvList() => ReservationListRequest.Raise(new DataPassingNotification());

        #endregion

        #region OnMouseDownCommand

        private ICommand _onMouseDownCommand;

        public ICommand OnMouseDownCommand =>
            _onMouseDownCommand ?? (_onMouseDownCommand = new DelegateCommand<MouseButtonEventArgs>(OnMouseDown));

        private void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2)
                return;
            var vm = ((FrameworkElement) e.Source).DataContext as ProgramCellViewModel;
            ProgramDetailsRequest.Raise(new DataPassingNotification {Model = vm?.Model});
        }

        #endregion
    }
}