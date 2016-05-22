using System;

using Prism.Interactivity.InteractionRequest;

namespace Norma.Interactivity
{
    internal class InteractionRequestedEventArgs2 : InteractionRequestedEventArgs
    {
        public new WindowNotification Context { get; private set; }

        public InteractionRequestedEventArgs2(WindowNotification context, Action callback) : base(null, callback)
        {
            Context = context;
        }
    }
}