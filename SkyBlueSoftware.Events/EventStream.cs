﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class EventStream : IEventStream
    {
        private IEnumerable<ISubscription> subscriptions;
        private readonly IDependencyContainer container;

        public EventStream(IDependencyContainer? container = null)
        {
            subscriptions = new ISubscription[] { };
            this.container = container ?? new DefaultDependencyContainer();
        }

        public EventStream Initialize(params ISubscribeTo[] subscribers) => Initialize(subscribers.AsEnumerable());
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

        public async Task Publish<T>(params object[] args) 
        {
            foreach (var o in subscriptions) await o.On(container.Create<T>(args)); 
        }

        #region IEnumerable

        public IEnumerator<ISubscription> GetEnumerator() => subscriptions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}