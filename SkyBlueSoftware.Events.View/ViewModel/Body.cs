using System.Collections.Generic;

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
