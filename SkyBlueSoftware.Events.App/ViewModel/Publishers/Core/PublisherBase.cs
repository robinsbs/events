using System.Windows.Input;

namespace SkyBlueSoftware.Events.App
{
    public abstract class PublisherBase : ViewModelBase
    {
        private readonly IEventStream events;

        public PublisherBase(IEventStream events)
        {
            this.events = events;
        }

        public string Label => $"{GetType().Name} {Name}";
        public abstract string Name { get; }
        public ICommand PublishCommand => Do(async () => await events.Publish(CreateEvent()));

        protected abstract object CreateEvent();
    }
}
