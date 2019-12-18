# Sky Blue Software - Events
An event framework for .NET Core client applications.

[![Build status](https://dev.azure.com/skybluesoftware/SBS/_apis/build/status/SkyBlueSoftware.Events)](https://dev.azure.com/skybluesoftware/SBS/_build/latest?definitionId=8)

```C#
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
        events.Initialize(list, detail);
        list.Select();
        detail.Change();
        Assert.AreEqual("DetailViewModel received SelectedEvent", detail.Message);
        Assert.AreEqual("ListViewModel received ChangedEvent", list.Message);
    }
}

class SelectedEvent { }
class ChangedEvent { }

class ListViewModel : ViewModelBase, ISubscribeTo<ChangedEvent>
{
    private readonly IEventStream events;
    public ListViewModel(IEventStream events) { this.events = events; }
    public async Task On(ChangedEvent e) => await Log(e);
    public void Select() => events.Publish(new SelectedEvent());
}

class DetailViewModel : ViewModelBase, ISubscribeTo<SelectedEvent>
{
    private readonly IEventStream events;
    public DetailViewModel(IEventStream events) { this.events = events; }
    public async Task On(SelectedEvent e) => await Log(e);
    public void Change() => events.Publish(new ChangedEvent());
}

abstract class ViewModelBase
{
    public ViewModelBase() => Message = string.Empty;
    public string Message { get; private set; }
    protected async Task Log(object e) { Message = $"{GetType().Name} received {e?.GetType().Name}"; await Task.CompletedTask; }
}
```
