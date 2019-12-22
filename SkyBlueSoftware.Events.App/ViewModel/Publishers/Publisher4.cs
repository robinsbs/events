namespace SkyBlueSoftware.Events.ViewModel
{
    public class Publisher4 : PublisherBase
    {
        public Publisher4(IEventStream events) : base(events) { }
        public override string Name => nameof(Event4);
        protected override object CreateEvent() => new Event4();
    }
}
