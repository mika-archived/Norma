using System;

namespace Norma.Interactivity
{
    internal class WindowNotification
    {
        public Type WindowType { get; private set; }

        public object Context { get; private set; }

        public WindowNotification(Type windowType, object context = null)
        {
            WindowType = windowType;
            Context = context;
        }
    }
}