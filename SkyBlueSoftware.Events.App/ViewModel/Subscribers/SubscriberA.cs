using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.ViewModel
{
    public class SubscriberA : SubscriberBase, ISubscribeTo<Event1>
    {
        public async Task On(Event1 e) => await LogEvent(e);
    }
}
