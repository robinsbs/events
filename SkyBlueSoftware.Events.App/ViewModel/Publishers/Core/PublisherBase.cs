using System.Windows.Input;

namespace SkyBlueSoftware.Events.App
{
    public abstract class PublisherBase : ViewModelBase
    {
        private readonly EventStream events;

        public PublisherBase(EventStream events)
        {
            this.events = events;
        }

        public string Label => $"{GetType().Name} {Name}";
        public abstract string Name { get; }
        public ICommand PublishCommand => Do(async () => await events.Publish(CreateEvent()));

        protected abstract object CreateEvent();
    }
}
