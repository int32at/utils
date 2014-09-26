using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using int32.Utils.Aggregator.Contracts;
using int32.Utils.Extensions;
using int32.Utils.Generics;

namespace int32.Utils.Aggregator
{
    public class Aggregator : Singleton<Aggregator>
    {
        private readonly Dictionary<Type, IList> _subscriptions;

        public Aggregator()
        {
            _subscriptions = new Dictionary<Type, IList>();
        }

        public void Subscribe<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            action.ThrowIfNull("action");

            var type = typeof(TEvent);
            var subscription = new Subscription<TEvent> { Type = type, Action = action };

            if (_subscriptions.ContainsKey(type))
                _subscriptions[type].Add(subscription);
            else
                _subscriptions.Add(type, new List<Subscription<TEvent>> { subscription });
        }

        public void Publish<TEvent>(TEvent message) where TEvent : IEvent
        {
            message.ThrowIfNull("message");

            var type = typeof(TEvent);

            if (!_subscriptions.ContainsKey(type)) return;

            foreach (var subscription in _subscriptions[type].Cast<Subscription<TEvent>>())
                subscription.Action(message);
        }

        public bool IsSubscribed<TEvent>() where TEvent : IEvent
        {
            var type = typeof(TEvent);
            return _subscriptions.ContainsKey(type);
        }
    }
}
