namespace SkyBlueSoftware.Events.App
{
    public class Publisher2 : PublisherBase
    {
        public Publisher2(EventStream events) : base(events) { }
        protected override object CreateEvent() => new Event2();
    }
}
