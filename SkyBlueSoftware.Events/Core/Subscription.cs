﻿// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    internal abstract class Subscription : ISubscription
    {
        protected Subscription(Type subscriberType, Type eventType)
        {
            Subscriber = subscriberType;
            Event = eventType;
            IsSubscribed = true;
        }

        public Type Subscriber { get; }
        public Type Event { get; }
        public int CallCount { get; protected set; }
        protected bool IsSubscribed { get; set; }
        protected bool IsNotSubscribed => !IsSubscribed;

        public ISubscription Unsubscribe() { IsSubscribed = false; return this; }
        public ISubscription Resubscribe() { IsSubscribed = true; return this; }

        public override string ToString() => $"{Subscriber.Name} subscribed to {Event.Name} has been called {CallCount} times.";

        public abstract Task On(object o);
    }

    internal class Subscription<T> : Subscription
    {
        private readonly ISubscribeTo<T> subscriber;

        public Subscription(ISubscribeTo<T> subscriber) : base(subscriber.GetType(), typeof(T))
        {
            this.subscriber = subscriber;
        }

        public override async Task On(object o)
        {
            if (IsSubscribed && o is T e)
            {
                await subscriber.On(e);
                CallCount += 1;
            }
        }
    }
}