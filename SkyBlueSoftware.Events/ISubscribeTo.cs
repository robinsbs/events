using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public interface ISubscribeTo { }
    public interface ISubscribeTo<in T> : ISubscribeTo { Task On(T e); }
}