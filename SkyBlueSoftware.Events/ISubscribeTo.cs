using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public interface ISubscribeTo { }
    public interface ISubscribeTo<in T> : ISubscribeTo { Task On(T e); }

    public interface ISubscribeToSync : ISubscribeTo { }
    public interface ISubscribeToSync<in T> : ISubscribeToSync { void On(T e); }
}