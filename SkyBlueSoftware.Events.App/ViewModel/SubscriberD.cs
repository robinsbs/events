using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.App
{
    public class SubscriberD : SubscriberBase, ISubscribeTo<Event3>, ISubscribeTo<Event4>
    {
        public async Task On(Event3 e) => await LogEvent(e);
        public async Task On(Event4 e) => await LogEvent(e);
    }
}
