using System.Collections.Generic;
using System.Windows.Input;

namespace SkyBlueSoftware.Events.App
{
    public class Body : ViewModelBase
    {
        private readonly EventStream events;

        public Body(IEnumerable<ISubscribeTo> subscribers, EventStream events)
        {
            Subscribers = subscribers;
            this.events = events;
        }

        public IEnumerable<ISubscribeTo> Subscribers { get; }
        public ICommand Event1Command => Do(async () => await events.Publish(new Event1()));
    }
}
