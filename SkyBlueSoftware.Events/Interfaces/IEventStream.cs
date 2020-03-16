// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public interface IEventStream : IEnumerable<ISubscription>
    {
        Task Publish<T>(params object[] args);
    }
}