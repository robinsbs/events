using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class EventStream : IEventStream
    {
        private readonly IReadOnlyCollection<ISubscription> subscriptions;

        public static IEventStream Create() => new EventStream(new List<ISubscription>());

        public EventStream(IReadOnlyCollection<ISubscription> subscriptions)
        {
            this.subscriptions = subscriptions;
        }

        public IEventStream Subscribe(params ISubscribeTo[] subscribers) => new EventStream(subscribers.SelectMany(x => x.CreateSubscriptions()).ToArray());

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