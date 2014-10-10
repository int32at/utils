using System;
using int32.Utils.Core.Aggregator.Contracts;

namespace int32.Utils.Core.Aggregator
{
    public class Subscription<TEvent> where TEvent : IEvent
    {
        public Type Type { get; set; }
        public Action<TEvent> Action { get; set; }
    }
}
