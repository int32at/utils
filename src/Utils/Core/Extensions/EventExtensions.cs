using System;

namespace int32.Utils.Core.Extensions
{
    public static class EventExtensions
    {
        public static void Raise(this EventHandler handler, object sender, EventArgs args)
        {
            if (handler != null)
                handler(sender, args);
        }

        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            if (eventHandler == null) return;
            eventHandler(sender, eventArgs);
        }
    }
}
