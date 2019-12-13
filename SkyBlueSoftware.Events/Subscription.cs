using System;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public abstract class Subscription : ISubscription
    {
        protected Subscription(Type subscriberType, Type eventType)
        {
            Subscriber = subscriberType;
            Event = eventType;
            IsSubscribed = true;
        }

        public Type Subscriber { get; }
        public Type Event { get; }
        public int CallCount { get; protected set; }
        protected bool IsSubscribed { get; set; }
        protected bool IsNotSubscribed => !IsSubscribed;

        public ISubscription Unsubscribe() { IsSubscribed = false; return this; }
        public ISubscription Resubscribe() { IsSubscribed = true; return this; }

        public override string ToString() => $"{Subscriber.Name} subscribed to {Event.Name} has been called {CallCount} times.";

        public abstract Task On(object o);
    }
}