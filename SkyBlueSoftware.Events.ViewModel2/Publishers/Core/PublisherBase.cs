using System.Windows.Input;

namespace SkyBlueSoftware.Events.ViewModel
{
    public abstract class PublisherBase : ViewModelBase, IPublisher
    {
        protected readonly IEventStream events;

        public PublisherBase(IEventStream events)
        {
            this.events = events;
        }

        public string Label => $"{GetType().Name} {Name}";
        public abstract string Name { get; }
        public abstract ICommand PublishCommand { get; }
    }
}
