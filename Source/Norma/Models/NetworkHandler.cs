using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Norma.Models
{
    internal class NetworkHandler
    {
        private readonly List<Tuple<Func<NetworkEventArgs, bool>, INetworkCaptureRequestAware>> _requestAwares;
        private readonly List<Tuple<Func<NetworkEventArgs, bool>, INetworkCaptureResponseAware>> _responseAwares;

        public NetworkHandler()
        {
            _requestAwares = new List<Tuple<Func<NetworkEventArgs, bool>, INetworkCaptureRequestAware>>();
            _responseAwares = new List<Tuple<Func<NetworkEventArgs, bool>, INetworkCaptureResponseAware>>();
        }

        private Tuple<Func<NetworkEventArgs, bool>, T> Create<T>(Func<NetworkEventArgs, bool> condition, T instance)
            => new Tuple<Func<NetworkEventArgs, bool>, T>(condition, instance);

        public void RegisterInstance(INetworkCaptureRequestAware instance, Func<NetworkEventArgs, bool> condition)
            => _requestAwares.Add(Create(condition, instance));

        public void RegisterInstance(INetworkCaptureResponseAware instance, Func<NetworkEventArgs, bool> condition)
            => _responseAwares.Add(Create(condition, instance));

        public void OnHandlingRequest(NetworkEventArgs e)
        {
            Task.Run(() =>
            {
                foreach (var requestAware in _requestAwares)
                {
                    if (requestAware.Item1.Invoke(e))
                        requestAware.Item2.OnRequestHandling(e);
                }
            });
        }

        public void OnHandlingResponse(NetworkEventArgs e)
        {
            Task.Run(() =>
            {
                foreach (var responseAware in _responseAwares)
                {
                    if (responseAware.Item1.Invoke(e))
                        responseAware.Item2.OnResponseHandling(e);
                }
            });
        }
    }
}