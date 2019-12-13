using System.Collections.Generic;

namespace SkyBlueSoftware.Events
{
    public interface ISubscriptions : IEnumerable<ISubscription>, IEventStream
    {

    }
}