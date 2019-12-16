using System.Collections.Generic;

namespace SkyBlueSoftware.Events.App
{
    public class Body
    {
        public Body(IEnumerable<ISubscribeTo> subscribers)
        {
            Subscribers = subscribers;
        }

        public IEnumerable<ISubscribeTo> Subscribers { get; }
    }
}
