namespace SkyBlueSoftware.Events.App
{
    public class Publisher2 : PublisherBase
    {
        public Publisher2(IEventStream events) : base(events) { }
        public override string Name => nameof(Event2);
        protected override object CreateEvent() => new Event2();
    }
}
