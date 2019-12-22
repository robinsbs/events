using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.ViewModel
{
    public class SubscriberC : SubscriberBase, ISubscribeTo<Event2>, ISubscribeTo<Event3>, ISubscribeTo<Event4>
    {
        public async Task On(Event2 e) => await LogEvent(e);
        public async Task On(Event3 e) => await LogEvent(e);
        public async Task On(Event4 e) => await LogEvent(e);
    }
}
