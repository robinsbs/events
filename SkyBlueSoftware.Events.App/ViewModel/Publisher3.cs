namespace SkyBlueSoftware.Events.App
{
    public class Publisher3 : PublisherBase
    {
        public Publisher3(EventStream events) : base(events) { }
        protected override object CreateEvent() => new Event3();
    }
}
