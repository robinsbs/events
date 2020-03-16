// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Autofac;
using System.Linq;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.Autofac
{
    public class AutofacDependencyContainer : IDependencyContainer
    {
        private readonly IContainer container;

        public AutofacDependencyContainer(IContainer container)
        {
            this.container = container;
        }
        public Task<T> Create<T>(params object[] args)
        {
            var parameters = args.Where(x => x != null).Select(x => new TypedParameter(x.GetType(), x)).ToArray();
            var o = container.Resolve<T>(parameters);
            return Task.FromResult(o);
        }
    }
}
