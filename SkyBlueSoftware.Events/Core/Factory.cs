using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class Factory : IFactory
    {
        private readonly IDependencyContainer container;
        private readonly IEventStream events;

        public Factory(IDependencyContainer container, IEventStream events)
        {
            this.container = container;
            this.events = events;
        }

        public async Task<T> Create<T>(params object[] args) => await container.Create<T>(args);

        public async Task Publish<T>(params object[] args) => await events.Publish<T>(args);
    }
}