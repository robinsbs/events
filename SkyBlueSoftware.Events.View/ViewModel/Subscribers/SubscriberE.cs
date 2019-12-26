using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.ViewModel
{
    public class SubscriberE : SubscriberBase, ISubscribeTo<Event4>
    {
        public async Task On(Event4 e) => await LogEvent(e);
    }
}
