using SkyBlueSoftware.Events.Autofac;
using System.Windows.Input;

namespace SkyBlueSoftware.Events.ViewModel
{
    public interface IPublisher : IRequireRegistration
    {
        string Label { get; }
        string Name { get; }
        ICommand PublishCommand { get; }
    }
}