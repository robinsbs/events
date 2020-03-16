// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using SkyBlueSoftware.Events.Autofac;

namespace SkyBlueSoftware.Events.ViewModel
{
    public class Body : ViewModelBase, IRequireRegistration
    {
        public Body(IEnumerable<IPublisher> publishers, IEnumerable<ISubscriber> subscribers)
        {
            Publishers = publishers;
            Subscribers = subscribers;
        }

        public IEnumerable<IPublisher> Publishers { get; }
        public IEnumerable<ISubscriber> Subscribers { get; }
    }
}
