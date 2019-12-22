using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public interface IEventStream : IEnumerable<ISubscription>
    {
        Task Publish<T>(T e);
        Task Publish<T>(params object[] args);
    }
}