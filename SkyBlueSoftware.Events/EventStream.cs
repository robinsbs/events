using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class EventStream : IEventStream
    {
        private readonly ICollection<Subscription> subscriptions;

        public static IEventStream Create() => new EventStream(new List<Subscription>());

        public EventStream(ICollection<Subscription> subscriptions)
        {
            this.subscriptions = subscriptions;
        }

        public ISubscriptions Subscribe(params ISubscribeTo[] subscribers)
        {
            var r = new Subscriptions(new List<ISubscription>(), this);
            foreach(var s in subscribers)
            {
                var interfaces = s.GetType().GetInterfaces().Where(x => x.IsGenericType && typeof(ISubscribeTo).IsInstanceOfType(s));
                foreach (var def in interfaces)
                {
                    var eventType = def.GetGenericArguments().FirstOrDefault();
                    if (eventType != null)
                    {
                        var genericType = typeof(Subscription<>).MakeGenericType(eventType);
                        var instance = Activator.CreateInstance(genericType, new[] { s });
                        if (instance is Subscription es)
                        {
                            subscriptions.Add(es);
                            r.Add(es);
                        }
                    }
                }
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