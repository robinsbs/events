// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.ViewModel
{
    public class SubscriberB : SubscriberBase, ISubscribeTo<Event1>, ISubscribeTo<Event2>
    {
        public async Task On(Event1 e) => await LogEvent(e);
        public async Task On(Event2 e) => await LogEvent(e);
    }
}
