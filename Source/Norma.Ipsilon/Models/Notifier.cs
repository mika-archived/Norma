using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Models;
using Norma.Delta.Models.Enums;
using Norma.Delta.Services;
using Norma.Eta.Extensions;
using Norma.Eta.Models;

namespace Norma.Ipsilon.Models
{
    internal class Notifier : IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable;
        private readonly DatabaseService _databaseService;
        private readonly int _pretime;
        private readonly List<Slot> _slot;

        public Notifier()
        {
            var configuration = ServiceLocator.Current.GetInstance<Configuration>();
            _databaseService = ServiceLocator.Current.GetInstance<DatabaseService>();
            _compositeDisposable = new CompositeDisposable();
            _slot = new List<Slot>();
            _pretime = (int) configuration.Root.Operation.ToastNotificationBeforeMinutes;
        }

        #region Implementation of IDisposable

        public void Dispose() => _compositeDisposable.Dispose();

        #endregion

        internal void Start()
        {
            // 10 秒に 1 回チェック
            _compositeDisposable.Add(Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10))
                                               .Subscribe(async w => await Check()));
            // 1 分に 1 回保存
            _compositeDisposable.Add(Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(1))
                                               .Subscribe(w => Clear()));
        }

        private async Task Check()
        {
            var list = new List<Slot>();
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start(); // SQL start
#endif
            using (var connection = _databaseService.Connect())
            {
                // 予約リスト
                // SELECT 句の実行自体は、数千件単位のものでも (SELECT + INSERT) で ~ 1s だし、いいかな
                var reservations = connection.Reservations.Where(w => w.IsEnabled); // Base query

                var keywords = reservations.Select(w => w.KeywordReservation).Where(w => w != null).ToList();
                var time = reservations.Select(w => w.TimeReservation).Where(w => w != null).ToList();
                var series = reservations.Select(w => w.SeriesReservation).Where(w => w != null).ToList();
                var slot1 = reservations.Select(w => w.SlotReservation).Where(w => w != null).ToList();
                var slot2 = reservations.Select(w => w.SlotReservation2).Where(w => w != null).ToList();

                // 予定
                var now = DateTime.Now;
                var perTime = DateTime.Now.AddMinutes(_pretime);
                var slots = connection.Slots.AsNoTracking().Where(w => now <= w.StartAt && w.StartAt <= perTime)
                                      .Include(w => w.Channel).ToList();
                foreach (var keyword in keywords)
                {
                    if (keyword.IsRegex)
                        slots.Where(w => new Regex(keyword.Keyword).IsMatch(w.Title)).ForEach(w => _slot.AddIfNotExists(list, w));
                    else
                        slots.Where(w => w.Title.Contains(keyword.Keyword)).ForEach(w => _slot.AddIfNotExists(list, w));
                }
                foreach (var tr in time)
                    slots.Where(w => w.StartAt == tr.StartAt)
                         .Where(w => tr.Repetition.IsMatch(w.StartAt))
                         .ForEach(w => _slot.AddIfNotExists(list, w));
                foreach (var st in series)
                    slots.Where(w => w.Episodes.First().Series.SeriesId == st.Series.SeriesId).ForEach(w => _slot.AddIfNotExists(list, w));
                foreach (var sr in slot1)
                    slots.Where(w => w.SlotId == sr.Slot.SlotId).ForEach(w => _slot.AddIfNotExists(list, w));
                foreach (var sr in slot2)
                    slots.Where(w => w.SlotId == sr.SlotId).ForEach(w => _slot.AddIfNotExists(list, w));
            }
#if DEBUG
            stopwatch.Stop();
            Debug.WriteLine(stopwatch.Elapsed.ToString());
#endif
            var uniqueList = list.Distinct().ToList();
            foreach (var slot in uniqueList)
            {
                _slot.Add(slot);
                await NotificationManager.ShowNotification(slot);
            }
        }

        private void Clear()
        {
            using (var connection = _databaseService.Connect())
            {
                var now = DateTime.Now;
                var reservations = connection.Reservations.Where(w => w.IsEnabled); // Base query
                var slots = connection.Slots.AsNoTracking().Where(w => w.StartAt <= now).ToList();

                var time = reservations.Select(w => w.TimeReservation).Where(w => w != null).ToList();
                var slot1 = reservations.Select(w => w.SlotReservation).Where(w => w != null).ToList();
                var slot2 = reservations.Select(w => w.SlotReservation2).Where(w => w != null).ToList();
                foreach (var tr in time)
                {
                    if (tr.Repetition == Repetition.None && tr.StartAt <= DateTime.Now)
                        tr.Reservation.IsEnabled = false;
                }
                foreach (var sr in slot1)
                {
                    if (sr.Slot.StartAt <= DateTime.Now)
                        sr.Reservation.IsEnabled = false;
                }
                foreach (var sr in slot2)
                {
                    if (slots.Single(w => w.SlotId == sr.SlotId).StartAt <= DateTime.Now)
                        sr.Reservation.IsEnabled = false;
                }

                connection.DetectChanges();
                connection.SaveChanges();
            }
        }
    }
}