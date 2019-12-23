using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public interface IDependencyContainer
    {
        Task<T> Create<T>(params object[] args);
    }
}