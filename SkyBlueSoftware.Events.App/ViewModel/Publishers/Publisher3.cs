namespace SkyBlueSoftware.Events.App
{
    public class Publisher3 : PublisherBase
    {
        public Publisher3(IEventStream events) : base(events) { }
        public override string Name => nameof(Event3);
        protected override object CreateEvent() => new Event3();
    }
}
