using System;

namespace Norma.Eta.Models
{
    public class ConnectOps : IDisposable
    {
        private IDisposable _disposable;

        #region Implementation of IDisposable

        public void Dispose() => _disposable.Dispose();

        #endregion

        public void Start()
        {

        }
    }
}