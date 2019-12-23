using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public interface IEventStream : IEnumerable<ISubscription>
    {
        Task Publish<T>(params object[] args);
    }
}