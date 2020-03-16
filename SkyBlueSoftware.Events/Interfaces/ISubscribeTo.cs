// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public interface ISubscribeTo { }
    public interface ISubscribeTo<in T> : ISubscribeTo { Task On(T e); }
}