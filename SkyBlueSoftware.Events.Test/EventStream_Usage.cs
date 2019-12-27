using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.Events;
using SkyBlueSoftware.Events.Autofac;
using static SkyBlueSoftware.Events.Autofac.SkyBlueSoftwareEvents;

[TestClass]
public class EventStream_Usage
{
    [TestMethod]
    public void EventStream_Usage_Example()
    {
        var (events, list, detail) = Initialize(this).Resolve<IEventStream, ListViewModel, DetailViewModel>();
        events.Publish<SelectedEvent>();
        events.Publish<ChangedEvent>();
        Assert.AreEqual("DetailViewModel received SelectedEvent", detail.Message);
        Assert.AreEqual("ListViewModel received ChangedEvent", list.Message);
    }
}

public class SelectedEvent : IRequireRegistrationNew { }
public class ChangedEvent : IRequireRegistrationNew { }

public class ListViewModel : ViewModelBase, ISubscribeTo<ChangedEvent>
{
    public async Task On(ChangedEvent e) => await Log(e);
}

public class DetailViewModel : ViewModelBase, ISubscribeTo<SelectedEvent>
{
    public async Task On(SelectedEvent e) => await Log(e);
}

public class ViewModelBase : IRequireRegistration
{
    public ViewModelBase() => Message = string.Empty;
    public string Message { get; private set; }
    protected Task Log(object e) 
    { 
        Message = $"{GetType().Name} received {e?.GetType().Name}"; 
        return Task.CompletedTask; 
    }
}
