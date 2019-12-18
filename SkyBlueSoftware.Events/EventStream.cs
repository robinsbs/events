using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class EventStream : IEventStream
    {
        private IEnumerable<ISubscription> subscriptions;

        public EventStream()
        {
            subscriptions = new ISubscription[] { };
        }

        public EventStream Initialize(IEnumerable<ISubscribeTo> subscribers)
        {
            subscriptions = subscribers.CreateSubscriptions();
            return this;
        }

        public async Task Publish<T>(T e) 
        {
            if (e == null) return;
            foreach (var o in subscriptions) await o.On(e); 
        }

        #region IEnumerable

        public IEnumerator<ISubscription> GetEnumerator() => subscriptions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}