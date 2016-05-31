using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Norma.Eta.Models;
using Norma.Eta.Models.Reservations;

namespace Norma.Ipsilon.Models
{
    internal class Notifier : IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable;
        private readonly Reservation _reservation;
        private readonly Timetable _timetable;
        private readonly Watcher _watcher;

        public Notifier(Timetable timetable, Reservation reservation)
        {
            _timetable = timetable;
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
            _compositeDisposable.Add(
                                     Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                                               .Subscribe(async w => await Check()));
        }

        private async Task Check()
        {
            var now = DateTime.Now;
            foreach (var program in _reservation.RsvsByProgram) {}

            await Notify(null);
        }

        private async Task Notify(Reserve reserve)
        {
            await NotificationManager.ShowNotification("", "");
        }

        private bool EqualityWithoutDate(DateTime obj1, DateTime obj2) =>
            obj1.Hour == obj2.Hour && obj1.Minute == obj2.Minute && obj1.Second == obj2.Second;
    }
}