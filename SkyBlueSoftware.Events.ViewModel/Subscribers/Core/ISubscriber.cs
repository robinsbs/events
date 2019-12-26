using SkyBlueSoftware.Events.Autofac;
using System.Collections.ObjectModel;

namespace SkyBlueSoftware.Events.ViewModel
{
    public interface ISubscriber : IRequireRegistration
    {
        string Delay { get; set; }
        ObservableCollection<string> Log { get; }
        string Name { get; }
    }
}