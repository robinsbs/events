// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Windows.Input;

namespace SkyBlueSoftware.Events.ViewModel
{
    public abstract class PublisherBase : ViewModelBase, IPublisher
    {
        protected readonly IEventStream events;

        protected PublisherBase(IEventStream events)
        {
            this.events = events;
        }

        public string Label => $"{GetType().Name} {Name}";
        public abstract string Name { get; }
        public abstract ICommand PublishCommand { get; }
    }
}
