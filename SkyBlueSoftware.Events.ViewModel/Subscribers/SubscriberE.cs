﻿// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.ViewModel
{
    public class SubscriberE : SubscriberBase, ISubscribeTo<Event4>
    {
        public async Task On(Event4 e) => await LogEvent(e);
    }
}
