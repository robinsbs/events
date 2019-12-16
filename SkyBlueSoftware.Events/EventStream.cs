using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class EventStream : IEventStream
    {
        private readonly IEnumerable<ISubscription> subscriptions;

        public EventStream(IEnumerable<ISubscribeTo> subscribers) => subscriptions = subscribers.CreateSubscriptions();

        public async Task Publish<T>(T e) { foreach (var o in subscriptions) await o.On(e); }

        #region IEnumerable

        public IEnumerator<ISubscription> GetEnumerator() => subscriptions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}