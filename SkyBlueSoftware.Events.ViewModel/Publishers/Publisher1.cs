namespace SkyBlueSoftware.Events.ViewModel
{
    public class Publisher1 : Publisher<Event1>
    {
        public Publisher1(IEventStream events) : base(events) { }
    }
}
