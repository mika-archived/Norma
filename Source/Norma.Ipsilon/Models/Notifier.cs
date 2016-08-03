using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Models;
using Norma.Delta.Services;
using Norma.Eta.Models;

namespace Norma.Ipsilon.Models
{
    internal class Notifier : IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable;
        private readonly Configuration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly int _pretime;
        private readonly List<Slot> _slot;

        public Notifier()
        {
            _configuration = ServiceLocator.Current.GetInstance<Configuration>();
            _databaseService = ServiceLocator.Current.GetInstance<DatabaseService>();
            _compositeDisposable = new CompositeDisposable();
            _slot = new List<Slot>();
            _pretime = (int) _configuration.Root.Operation.ToastNotificationBeforeMinutes;
        }

        #region Implementation of IDisposable

        public void Dispose() => _compositeDisposable.Dispose();

        #endregion

        internal void Start()
        {
            // 10 秒に 1 回チェック
            _compositeDisposable.Add(Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10))
                                               .Subscribe(async w => await Check()));
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

                var keywords = reservations.Select(w => w.KeywordReservation).ToList();
                var time = reservations.Select(w => w.TimeReservation).ToList();
                var series = reservations.Select(w => w.SeriesReservation).ToList();
                var slot1 = reservations.Select(w => w.SlotReservation).ToList();
                var slot2 = reservations.Select(w => w.SlotReservation2).ToList();

                // 予定
                var now = DateTime.Now;
                var perTime = DateTime.Now.AddMinutes(_pretime);
                var slots = connection.Slots.Where(w => now <= w.StartAt && w.StartAt <= perTime).ToList();
                foreach (var keyword in keywords)
                {
                    if (keyword.IsRegex)
                        slots.Where(w => new Regex(keyword.Keyword).IsMatch(w.Title)).ForEach(w => list.Add(w));
                    else
                        slots.Where(w => w.Title.Contains(keyword.Keyword)).ForEach(w => list.Add(w));
                }
                foreach (var tr in time)
                    slots.Where(w => w.StartAt == tr.StartAt).ForEach(w => list.Add(w));
                foreach (var st in series)
                    slots.Where(w => w.Episodes.First().Series.SeriesId == st.Series.SeriesId).ForEach(w => list.Add(w));
                foreach (var sr in slot1)
                    slots.Where(w => w.SlotId == sr.Slot.SlotId).ForEach(w => list.Add(w));
                foreach (var sr in slot2)
                    slots.Where(w => w.SlotId == sr.SlotId).ForEach(w => list.Add(w));
            }
#if DEBUG
            stopwatch.Stop();
            Debug.WriteLine(stopwatch.Elapsed.ToString());
            Debug.WriteLine("");
#endif
            // TODO: Notification
        }
    }
}