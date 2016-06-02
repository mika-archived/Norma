using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;

using Norma.Eta;
using Norma.Eta.Models;
using Norma.Eta.Models.Operations;

namespace Norma.Models
{
    /// <summary>
    ///     飛んできたリクエストを、 VM にぶん投げる
    /// </summary>
    internal class Connector : IDisposable
    {
        private readonly ConnectOps _connectOps;
        private readonly Dictionary<Type, IOperationRequestAware> _operationTables;
        private IDisposable _disposable;

        public Connector(ConnectOps connectOps)
        {
            _connectOps = connectOps;
            _operationTables = new Dictionary<Type, IOperationRequestAware>();
            Start();
        }

        #region Implementation of IDisposable

        public void Dispose() => _disposable.Dispose();

        #endregion

        public void RegisterInsance<T>(IOperationRequestAware instance) where T : IOperation
        {
            _operationTables.Add(typeof(T), instance);
        }

        private void Start()
        {
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Subscribe(w => Watch());
        }

        private void Watch()
        {
            _connectOps.Load();
            if (_connectOps.Operation == null)
                return;
            var operation = _connectOps.Operation;
            if (!_operationTables.ContainsKey(operation.GetType()))
            {
                Debug.WriteLine($"[Warning]Does not registered {operation.GetType().Name}'s handler.");
                Debug.WriteLine($"[Warning]Request does not invoke.");
                File.Delete(NormaConstants.OpsFile);
                return;
            }
            _operationTables[operation.GetType()].Invoke(operation);
            File.Delete(NormaConstants.OpsFile);
        }
    }
}