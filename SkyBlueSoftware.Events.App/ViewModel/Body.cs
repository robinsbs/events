using System.Collections.Generic;

namespace SkyBlueSoftware.Events.App
{
    public class Body : ViewModelBase
    {
        public Body(IEnumerable<PublisherBase> publishers, IEnumerable<SubscriberBase> subscribers)
        {
            Publishers = publishers;
            Subscribers = subscribers;
        }

        public IEnumerable<PublisherBase> Publishers { get; }
        public IEnumerable<SubscriberBase> Subscribers { get; }
    }
}
