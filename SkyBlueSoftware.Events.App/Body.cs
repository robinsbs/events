using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SkyBlueSoftware.Events.App
{
    public class Body
    {
        private readonly EventStream events;

        public Body(IEnumerable<ISubscribeTo> subscribers, EventStream events)
        {
            Subscribers = subscribers;
            this.events = events;
        }

        public IEnumerable<ISubscribeTo> Subscribers { get; }
        public ICommand Event1Command => new Command(async () => await events.Publish(new Event1()));
    }

    public class Command : ICommand
    {
        private readonly Action action;

        public Command(Action action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged = (o, e) => { };

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => action();
    }
}
