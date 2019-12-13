using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public interface IEventStream : IEnumerable<ISubscription>
    {
        ISubscriptions Subscribe(params ISubscribeTo[] subscribers);
        Task Publish<T>(T e);
    }
}