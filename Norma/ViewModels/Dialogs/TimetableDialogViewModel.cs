using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

using Norma.Extensions;
using Norma.ViewModels.Internal;
using Norma.ViewModels.Timetable;

using ModelTimetable = Norma.Models.Timetable;

namespace Norma.ViewModels.Dialogs
{
    internal class TimetableDialogViewModel : ViewModel
    {
        private readonly ModelTimetable _timetable;
        private int _index; // 日付管理用(0 = 今日, 6 = 一週間後みたいな)
        public ObservableCollection<ChannelViewModel> Channels { get; }
        public List<string> AvailableDates { get; }

        public TimetableDialogViewModel(ModelTimetable timetable)
        {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
            _timetable = timetable;
            _index = (DateTime.Now - timetable.LastSyncTime).Days;
            AvailableDates = new List<string>();
            Channels = new ObservableCollection<ChannelViewModel>();
            for (var i = 0; i < 7; i++)
                AvailableDates.Add(timetable.LastSyncTime.AddDays(i).ToString("MM/dd"));

            SelectedDate = AvailableDates[0];
        }

        private void UpdateChannels()
        {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);

            Channels.Clear();
            var list = new List<ChannelViewModel>();
            foreach (var channel in _timetable.Channels)
            {
                var slots = _timetable.ChannelSchedules.Where(w => w.ChannelId == channel.Id).ElementAt(_index);
                var vm = new ChannelViewModel(channel, slots.Slots, slots.Date).AddTo(this);
                list.Add(vm);
            }
            foreach (var vm in list)
                Channels.Add(vm);
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
                Observable.Return(0).Delay(TimeSpan.FromSeconds(1)).Subscribe(w => UpdateChannels());
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
    }
}