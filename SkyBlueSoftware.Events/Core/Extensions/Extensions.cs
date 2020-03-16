// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static IEnumerable<Type> SubscribedTo(this object o)
        {
            var interfaces = o.GetType()
                              .GetInterfaces()
                              .Where(x => x.IsGenericType && x.GetInterfaces()
                                                              .Any(y => y.IsAssignableFrom(typeof(ISubscribeTo))));
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
                if (instance is ISubscription s) yield return s;
            }
        }

        public static ISubscription[] CreateSubscriptions(this IEnumerable<ISubscribeTo> subscribers)
        {
            return subscribers.SelectMany(x => x.CreateSubscriptions()).ToArray();
        }

        public static string Delimit<T>(this IEnumerable<T> source, string delimiter)
        {
            if (source == null) return string.Empty;
            return string.Join(delimiter, source);
        }
    }
}