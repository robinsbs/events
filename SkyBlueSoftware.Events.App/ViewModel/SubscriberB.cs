using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.App
{
    public class SubscriberB : SubscriberBase, ISubscribeTo<Event1>, ISubscribeTo<Event2>
    {
        public async Task On(Event1 e) => await LogEvent(e);
        public async Task On(Event2 e) => await LogEvent(e);
    }
}
