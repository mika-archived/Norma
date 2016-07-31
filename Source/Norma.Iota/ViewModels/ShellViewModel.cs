using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

using Norma.Eta.Extensions;
using Norma.Eta.Models;
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
        private readonly Timetable _timetable;
        private int _index; // 日付管理用(0 = 今日, 6 = 一週間後みたいな)
        public ObservableCollection<ChannelCellViewModel> Channels { get; }
        public List<string> AvailableDates { get; }
        public InteractionRequest<INotification> ProgramDetailsRequest { get; }
        public InteractionRequest<INotification> ReservationListRequest { get; }
        public InteractionRequest<DataPassingNotification> DetailReserveRequest { get; }

        public ShellViewModel(Timetable timetable)
        {
            _timetable = timetable;
            ProgramDetailsRequest = new InteractionRequest<INotification>();
            ReservationListRequest = new InteractionRequest<INotification>();
            DetailReserveRequest = new InteractionRequest<DataPassingNotification>();
            _index = (DateTime.Now - timetable.LastSyncTime).Days;
            AvailableDates = new List<string>();
            Channels = new ObservableCollection<ChannelCellViewModel>();
            for (var i = 0; i < 6; i++)
                AvailableDates.Add(timetable.LastSyncTime.AddDays(i).ToString("MM/dd"));

            SelectedDate = AvailableDates[0];
        }

        private void UpdateChannels()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Channels.Clear();
                CompositeDisposable.Dispose();
            });
            var list = new List<ChannelCellViewModel>();
            foreach (var channel in _timetable.Channels)
            {
                var slots = _timetable.ChannelSchedules.Where(w => w.ChannelId == channel.Id).ElementAt(_index);
                var vm = new ChannelCellViewModel(channel, slots.Slots, slots.Date).AddTo(this);
                list.Add(vm);
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