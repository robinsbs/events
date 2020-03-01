using System.Windows.Input;

namespace SkyBlueSoftware.Events.ViewModel
{
    public abstract class Publisher<T> : PublisherBase
    {
        public Publisher(IEventStream events) : base(events) { }
        public override string Name => typeof(T).Name;
        public override ICommand PublishCommand => Do(async () => await events.Publish<T>());
    }
}
