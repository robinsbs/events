using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public interface IFactory
    {
        Task Publish<T>(params object[] args);
        Task<T> Create<T>(params object[] args);
    }
}