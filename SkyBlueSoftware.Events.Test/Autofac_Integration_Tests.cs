using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.TestFramework;

namespace SkyBlueSoftware.Events.Test
{
    [TestClass]
    public class Autofac_Integration_Tests
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public async Task Autofac_Integration_Tests_Test01()
        {
            var b = new ContainerBuilder();
            b.RegisterType<Widget>();
            var c = b.Build();
            int i = 1;
            string s = "s";
            DateTime d = new DateTime(2019, 12, 1, 3, 2, 1);
            var args = new object[] { i, s, d };
            var parameters = args.Select(x => new TypedParameter(x.GetType(), x)).ToArray();
            var w = c.Resolve<Widget>(parameters);
            t.Verify(w);
            await Task.CompletedTask;
        }

        [TestMethod]
        public async Task Autofac_Integration_Tests_Test02()
        {
            var b = new ContainerBuilder();
            b.RegisterType<Widget>();
            var c = b.Build();
            var f = new Factory(new AutofacDependencyContainer(c), new EventStream());
            int i = 1;
            string s = "s";
            var d = new DateTime(2019, 12, 1, 3, 2, 1);
            var w = await f.Create<Widget>(i, s, d);
            t.Verify(w);
        }

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

        public class Widget
        {
            public Widget(int i, string s, DateTime d)
            {
                I = i;
                S = s;
                D = d;
            }

            public int I { get; }
            public string S { get; }
            public DateTime D { get; }
        }
    }
}
