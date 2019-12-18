using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.Events;

[TestClass]
public class EventStream_Usage
{
    [TestMethod]
    public void EventStream_Usage_Example()
    {
        var events = new EventStream();
        events.Initialize(new[] { new DetailViewModel() });
        new ListViewModel(events).Select();
    }
}

class SelectedEvent { }

class ListViewModel
{
    private readonly IEventStream events;
    public ListViewModel(IEventStream events) { this.events = events; }
    public void Select() => events.Publish(new SelectedEvent());
}

class DetailViewModel : ISubscribeTo<SelectedEvent>
{
    public async Task On(SelectedEvent e) { Console.WriteLine($"{GetType().Name} received {e}"); await Task.CompletedTask; }
}
