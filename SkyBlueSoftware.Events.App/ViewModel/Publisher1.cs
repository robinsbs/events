namespace SkyBlueSoftware.Events.App
{
    public class Publisher1 : PublisherBase
    {
        public Publisher1(EventStream events) : base(events) { }
        protected override object CreateEvent() => new Event1();
    }
}
