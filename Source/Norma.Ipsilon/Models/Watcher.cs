using System;
using System.IO;
using System.Reactive.Linq;

using Norma.Eta;
using Norma.Eta.Models;

namespace Norma.Ipsilon.Models
{
    internal class Watcher : IDisposable
    {
        private readonly Reservation _reservation;
        private IDisposable _disposable;

        public Watcher(Reservation reservation)
        {
            _reservation = reservation;
        }

        #region Implementation of IDisposable

        public void Dispose() => _disposable.Dispose();

        #endregion

        public void Start()
        {
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Subscribe(w => Watch());
        }

        private void Watch()
        {
            if (!File.Exists(NormaConstants.ReserveProgramLockFile))
                return;
            try
            {
                File.Delete(NormaConstants.ReserveProgramLockFile);
                _reservation.Reload();
            }
            catch
            {
                // ignored
            }
        }
    }
}