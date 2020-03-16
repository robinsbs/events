// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Windows.Input;

namespace SkyBlueSoftware.Events.ViewModel
{
    public abstract class Publisher<T> : PublisherBase
    {
        protected Publisher(IEventStream events) : base(events) { }
        public override string Name => typeof(T).Name;
        public override ICommand PublishCommand => Do(async () => await events.Publish<T>());
    }
}
