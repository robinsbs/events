using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class EventStream : IEventStream
    {
        private readonly IReadOnlyCollection<ISubscription> subscriptions;

        public EventStream(IReadOnlyCollection<ISubscription> subscriptions)
        {
            this.subscriptions = subscriptions;
        }

        public static IEventStream Create(IReadOnlyCollection<ISubscription> subscriptions = null) => new EventStream(subscriptions ?? new ISubscription[] { });
        public IEventStream Subscribe(params ISubscribeTo[] subscribers) => Create(subscribers.CreateSubscriptions());
        public async Task Publish<T>(T e) { foreach (var o in subscriptions) await o.On(e); }

        #region IEnumerable

        public IEnumerator<ISubscription> GetEnumerator() => subscriptions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}