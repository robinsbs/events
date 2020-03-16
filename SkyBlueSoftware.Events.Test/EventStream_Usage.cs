// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
