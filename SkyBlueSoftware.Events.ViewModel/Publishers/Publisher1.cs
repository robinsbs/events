// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace SkyBlueSoftware.Events.ViewModel
{
    public class Publisher1 : Publisher<Event1>
    {
        public Publisher1(IEventStream events) : base(events) { }
    }
}
