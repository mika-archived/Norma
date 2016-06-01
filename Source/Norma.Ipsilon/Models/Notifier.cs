﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Norma.Eta.Models;
using Norma.Eta.Models.Reservations;
using Norma.Eta.Properties;
using Norma.Gamma.Models;

using static Norma.Eta.Models.DateTimeHelper;

namespace Norma.Ipsilon.Models
{
    internal class Notifier : IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable;
        private readonly Reservation _reservation;
        private readonly Timetable _timetable;
        private readonly List<ChannelSchedule> _todaySchedules;
        private readonly Watcher _watcher;

        private DateTime _lastSyncTime;

        public Notifier(Timetable timetable, Reservation reservation)
        {
            _timetable = timetable;
            _lastSyncTime = DateTime.Now;
            _todaySchedules = timetable.ChannelSchedules.Where(w => EqualsWithDates(w.Date, DateTime.Today))
                                       .ToList();
            _reservation = reservation;
            _watcher = new Watcher(reservation);
            _compositeDisposable = new CompositeDisposable {_watcher};
        }

        #region Implementation of IDisposable

        public void Dispose() => _compositeDisposable.Dispose();

        #endregion

        internal void Start()
        {
            _watcher.Start();
            _compositeDisposable.Add(Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1))
                                               .Subscribe(async w => await Check()));
        }

        private async Task Check()
        {
            var list = new List<Reserve>(); // 複数重なる場合もまぁ
            foreach (var program in _reservation.RsvsByProgram.Where(w => w.IsEnable))
            {
                if (IsNoticeable(program.StartDate))
                    list.Add(program);
            }
            foreach (var time in _reservation.RsvsByTime.Where(w => w.IsEnable))
            {
                if (time.DayOfWeek.IsMatch(DateTime.Now) && IsNoticeable(time.StartTime) &&
                    IsNoticeableRange(time.Range))
                    list.Add(time);
            }

            // Design Guide 的には、まとめたほうがよさ気。
            foreach (var reserve in list)
                await Notify(reserve);
            await KeywordNotify();
        }

        private async Task Notify(Reserve reserve)
        {
            string title;
            string body;
            if (reserve is RsvProgram)
            {
                title = Resources.NoticeSlotRsvTitle;
                _reservation.Reservations.Single(w => w.Id == reserve.Id).IsEnable = false;
                Slot slot = null;
                foreach (var schedule in _todaySchedules)
                {
                    if (schedule.Slots.Any(w => w.Id == ((RsvProgram) reserve).ProgramId))
                        slot = schedule.Slots.Single(w => w.Id == ((RsvProgram) reserve).ProgramId);
                }
                body = string.Format(Resources.NoticeSlotRsvBody, slot?.Title);
            }
            else
            {
                title = Resources.NoticeTimeRsvTitle;
                body = string.Format(Resources.NoticeTimeRsvBody, ((RsvTime) reserve).StartTime.ToString("t"));
            }
            await NotificationManager.ShowNotification(title, body);
        }

        private async Task KeywordNotify()
        {
            foreach (var keyword in _reservation.RsvsByKeyword.Where(w => w.IsEnable))
            {
                foreach (var schedule in _todaySchedules)
                {
                    var slots = schedule.Slots.Where(w => keyword.IsRegexMode
                        ? new Regex(keyword.Keyword).IsMatch(w.Title)
                        : w.Title == keyword.Keyword);
                    foreach (var slot in slots)
                    {
                        if (!IsNoticeable(slot.StartAt))
                            continue;
                        var title = Resources.NoticeKeywordRsvTitle;
                        var body = string.Format(Resources.NoticeKeywordRsvBody, keyword.Keyword);
                        await NotificationManager.ShowNotification(title, body);
                        return;
                    }
                }
            }
        }

        private bool IsNoticeable(DateTime dateTime)
            => dateTime >= DateTime.Now && DateTime.Now >= dateTime.AddMinutes(-5);

        private bool IsNoticeableRange(DateRange range) => range.Start <= DateTime.Now && DateTime.Now <= range.Finish;
    }
}