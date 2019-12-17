using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.App
{
    public class B : SubscriberBase, ISubscribeTo<Event2>
    {
        public async Task On(Event2 e) => await LogEvent(e);
    }
}
