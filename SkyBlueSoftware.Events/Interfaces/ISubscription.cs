// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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