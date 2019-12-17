﻿using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.App
{
    public class A : SubscriberBase, ISubscribeTo<Event1>
    {
        public async Task On(Event1 e) => await LogEvent(e);
    }
}
