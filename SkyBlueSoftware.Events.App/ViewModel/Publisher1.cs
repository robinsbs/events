namespace SkyBlueSoftware.Events.App
{
    public class Publisher1 : PublisherBase
    {
        public Publisher1(EventStream events) : base(events) { }
        public override string Name => nameof(Event1);
        protected override object CreateEvent() => new Event1();
    }
}
