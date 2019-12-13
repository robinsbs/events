using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class Subscriptions : ISubscriptions
    {
        private readonly ICollection<ISubscription> subscriptions;
        private readonly IEventStream eventStream;

        public Subscriptions(ICollection<ISubscription> subscriptions, IEventStream eventStream)
        {
            this.subscriptions = subscriptions;
            this.eventStream = eventStream;
        }

        public void Add(ISubscription subscription) => subscriptions.Add(subscription);
        public Task Publish<T>(T e) => eventStream.Publish(e);
        public ISubscriptions Subscribe(params ISubscribeTo[] subscribers) => eventStream.Subscribe(subscribers);

        public IEnumerator<ISubscription> GetEnumerator() => subscriptions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}