using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class EventStream : IEventStream
    {
        private readonly ICollection<ISubscription> subscriptions;

        public static IEventStream Create() => new EventStream(new List<ISubscription>());

        public EventStream(ICollection<ISubscription> subscriptions)
        {
            this.subscriptions = subscriptions;
        }

        public ISubscriptions Subscribe(params ISubscribeTo[] subscribers)
        {
            var r = new Subscriptions(new List<ISubscription>(), this);
            foreach(var s in subscribers)
            {
                s.CreateSubscriptions().ForEach(x => 
                {
                    subscriptions.Add(x);
                    r.Add(x);
                });
            }
            return r;
        }

        public async Task Publish<T>(T e)
        {
            if (e == null) return;
            foreach (var o in subscriptions)
            {
                await o.On(e);
            }
        }

        public IEnumerator<ISubscription> GetEnumerator() => subscriptions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}