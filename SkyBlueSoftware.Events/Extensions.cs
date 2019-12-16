using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static async Task Async(this Task t) => await t.ConfigureAwait(false);

        public static IEnumerable<Type> SubscribedTo(this object o)
        {
            var interfaces = o.GetType().GetInterfaces().Where(x => x.IsGenericType && typeof(ISubscribeTo).IsInstanceOfType(o));
            foreach (var i in interfaces)
            {
                foreach (var eventType in i.GetGenericArguments())
                {
                    yield return eventType;
                    break;
                }
            }
        }

        public static IEnumerable<ISubscription> CreateSubscriptions(this object o)
        {
            foreach (var t in o.SubscribedTo())
            {
                var genericType = typeof(Subscription<>).MakeGenericType(t);
                var instance = Activator.CreateInstance(genericType, new[] { o });
                yield return (ISubscription)instance;
            }
        }

        public static ISubscription[] CreateSubscriptions(this IEnumerable<ISubscribeTo> subscribers)
        {
            return subscribers.SelectMany(x => x.CreateSubscriptions()).ToArray();
        }
    }
}