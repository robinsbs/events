using System;

namespace SkyBlueSoftware.Events
{
    public interface ISubscription
    {
        Type Subscriber { get; }
        Type Event { get; }
        int CallCount { get; }
        ISubscription Unsubscribe();
        ISubscription Resubscribe();
    }
}