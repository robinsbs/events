// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    internal class DefaultDependencyContainer : IDependencyContainer
    {
        public Task<T> Create<T>(params object[] args)
        {
            var instance = Activator.CreateInstance(typeof(T), args);
            if (instance is T o) return Task.FromResult(o);
            throw new InvalidProgramException($"Unable to create an instance of {typeof(T).Name} with args: {args.Delimit(",")}");
        }
    }
}