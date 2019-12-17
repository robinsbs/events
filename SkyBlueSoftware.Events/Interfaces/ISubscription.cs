using System;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public interface ISubscription
    {
        Type Subscriber { get; }
        Type Event { get; }
        int CallCount { get; }
        ISubscription Unsubscribe();
        ISubscription Resubscribe();
        Task On(object o);
    }
}