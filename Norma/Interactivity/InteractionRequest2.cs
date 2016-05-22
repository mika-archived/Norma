using System;

using Prism.Interactivity.InteractionRequest;

namespace Norma.Interactivity
{
    internal class InteractionRequest2 : IInteractionRequest
    {
        #region Implementation of IInteractionRequest

        public event EventHandler<InteractionRequestedEventArgs> Raised;

        #endregion

        public void Raise(WindowNotification context)
            => Raise(context, callback => { });

        public void Raise(WindowNotification context, Action<object> callback)
        {
            var handler = Raised;
            handler?.Invoke(this, new InteractionRequestedEventArgs2(context, () => callback(context)));
        }
    }
}