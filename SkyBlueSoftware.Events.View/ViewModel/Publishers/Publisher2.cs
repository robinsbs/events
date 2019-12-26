namespace SkyBlueSoftware.Events.ViewModel
{
    public class Publisher2 : Publisher<Event2>
    {
        public Publisher2(IEventStream events) : base(events) { }
    }
}
