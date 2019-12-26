﻿using System;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class DefaultDependencyContainer : IDependencyContainer
    {
        public Task<T> Create<T>(params object[] args)
        {
            var instance = Activator.CreateInstance(typeof(T), args);
            if (instance is T o) return Task.FromResult(o);
            throw new InvalidProgramException($"Unable to create an instance of {typeof(T).Name} with args: {args.Delimit(",")}");
        }
    }
}