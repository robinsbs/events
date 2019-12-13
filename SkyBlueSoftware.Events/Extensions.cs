using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public static class Extensions
    {
        public static async Task Async(this Task t) => await t.ConfigureAwait(false);

        public static IEnumerable<Type> SubscribedTo(this object o)
        {
            var interfaces = o.GetType().GetInterfaces().Where(x => x.IsGenericType && typeof(ISubscribeTo).IsInstanceOfType(o));
            foreach (var i in interfaces)
            {
                foreach (var eventType in i.GetGenericArguments())
                {
                    if (eventType != null) yield return eventType;
                }
            }
        }
    }
}