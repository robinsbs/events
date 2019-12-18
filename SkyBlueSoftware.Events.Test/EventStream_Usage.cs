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
        var list = new ListViewModel(events);
        var detail = new DetailViewModel(events);
        events.Initialize(new ISubscribeTo[] { list, detail });
        list.Select();
        detail.Change();
    }
}

class SelectedEvent { }
class ChangedEvent { }

class ListViewModel : ISubscribeTo<ChangedEvent>
{
    private readonly IEventStream events;
    public ListViewModel(IEventStream events) { this.events = events; }
    public async Task On(ChangedEvent e) { Console.WriteLine($"{GetType().Name} received {e}"); await Task.CompletedTask; }
    public void Select() => events.Publish(new SelectedEvent());
}

class DetailViewModel : ISubscribeTo<SelectedEvent>
{
    private readonly IEventStream events;
    public DetailViewModel(IEventStream events) { this.events = events; }
    public async Task On(SelectedEvent e) { Console.WriteLine($"{GetType().Name} received {e}"); await Task.CompletedTask; }
    public void Change() => events.Publish(new ChangedEvent());
}
