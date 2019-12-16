using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public interface IEventStream : IEnumerable<ISubscription>
    {
        IEventStream Subscribe(params ISubscribeTo[] subscribers);
        Task Publish<T>(T e);
    }
}