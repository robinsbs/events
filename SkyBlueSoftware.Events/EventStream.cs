using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class EventStream : IEventStream
    {
        private readonly IReadOnlyCollection<ISubscription> subscriptions;

        public static IEventStream Create(IReadOnlyCollection<ISubscription> subscriptions = null) => new EventStream(subscriptions ?? new ISubscription[] { });

        public EventStream(IReadOnlyCollection<ISubscription> subscriptions)
        {
            this.subscriptions = subscriptions;
        }

        public IEventStream Subscribe(params ISubscribeTo[] subscribers) => EventStream.Create(subscribers.SelectMany(x => x.CreateSubscriptions()).ToArray());

        public async Task Publish<T>(T e)
        {
            foreach (var o in subscriptions)
            {
                await o.On(e);
            }
        }

        public IEnumerator<ISubscription> GetEnumerator() => subscriptions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}