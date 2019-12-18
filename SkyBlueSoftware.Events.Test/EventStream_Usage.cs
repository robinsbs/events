using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SkyBlueSoftware.Events.Test
{
    [TestClass]
    public class EventStream_Usage
    {
        [TestMethod]
        public void EventStream_Usage_Example()
        {
            var events = new EventStream(new[] { new DetailViewModel() });
            new ListViewModel(events).Select();
        }
    }

    public class SelectedEvent { }

    public class ListViewModel
    {
        private readonly IEventStream events;

        public ListViewModel(IEventStream events)
        {
            this.events = events;
        }

        public void Select() => events.Publish(new SelectedEvent());
    }

    public class DetailViewModel : ISubscribeTo<SelectedEvent>
    {
        public async Task On(SelectedEvent e)
        {
            Console.WriteLine($"Received {e}");
            await Task.CompletedTask;
        }
    }
}
