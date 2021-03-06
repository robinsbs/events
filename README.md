# Sky Blue Software - Events
An event framework for .NET Core client applications.

[![Build status](https://dev.azure.com/skybluesoftware/SBS/_apis/build/status/SkyBlueSoftware.Events)](https://dev.azure.com/skybluesoftware/SBS/_build/latest?definitionId=8)
[![Nuget](https://img.shields.io/nuget/v/SkyBlueSoftware.Events)](https://www.nuget.org/packages/SkyBlueSoftware.Events)

```C#
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.Events;
using SkyBlueSoftware.Events.Autofac;

[TestClass]
public class EventStream_Usage
{
    [TestMethod]
    public void EventStream_Usage_Example()
    {
        var (events, list, detail) = SkyBlueSoftwareEvents.Initialize<IApp>(this)
                                                          .Resolve<IEventStream, ListViewModel, DetailViewModel>();
        events.Publish<SelectedEvent>();
        events.Publish<ChangedEvent>();
        Assert.IsTrue(detail.IsSelected);
        Assert.IsTrue(list.IsChanged);
    }
}

public interface IApp { }
public class SelectedEvent : IApp { }
public class ChangedEvent : IApp { }

public class ListViewModel : IApp, ISubscribeTo<ChangedEvent>
{
    public bool IsChanged { get; private set; }
    public Task On(ChangedEvent e) => Task.FromResult(IsChanged = true);
}

public class DetailViewModel : IApp, ISubscribeTo<SelectedEvent>
{
    public bool IsSelected { get; private set; }
    public Task On(SelectedEvent e) => Task.FromResult(IsSelected = true);
}
```
