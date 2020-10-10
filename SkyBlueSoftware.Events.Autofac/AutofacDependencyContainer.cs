// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Autofac;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Core;

namespace SkyBlueSoftware.Events.Autofac
{
    public class AutofacDependencyContainer : IDependencyContainer
    {
        private readonly ILifetimeScope container;

        public AutofacDependencyContainer(ILifetimeScope container)
        {
            this.container = container;
        }
        public Task<T> Create<T>(params object[] args)
        {
            var parameters = args.Where(x => x != null).Select(x => new TypedParameter(x.GetType(), x)).OfType<Parameter>().ToArray();
            var o = container.Resolve<T>(parameters);
            return Task.FromResult(o);
        }
    }
}
