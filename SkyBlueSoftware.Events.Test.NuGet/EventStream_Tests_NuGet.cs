using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.TestFramework;
using System.Linq;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.Test.NuGet
{
    [TestClass]
    public class EventStream_Tests_NuGet
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public async Task EventStream_Tests_NuGet_Test01()
        {
            var events = ES(new A(), new B());
            await events.Publish<E1>();
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_NuGet_Test02()
        {
            var events = ES(new A(), new B());
            events.Where(x => x.Subscriber == typeof(A)).ForEach(x => x.Unsubscribe());
            await events.Publish<E>();
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_NuGet_Test03()
        {
            var events = ES(new C());
            await events.Publish<E2>();
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_NuGet_Test04()
        {
            var events = ES(new C());
            await events.Publish<E>();
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_NuGet_Test05()
        {
            var events = ES(new D());
            await events.Publish<E1>();
            await events.Publish<E2>();
            await events.Publish<E3>();
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_NuGet_Test06()
        {
            var events = ES(new A(), new B());
            events.Where(x => x.Subscriber == typeof(A)).FirstOrDefault().Unsubscribe().Resubscribe();
            await events.Publish<E>();
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_NuGet_Test07()
        {
            var a = new A();
            var events = a.SubscribedTo().ToArray();
            Assert.AreEqual(1, events.Length);
            Assert.AreEqual(typeof(E), events[0]);
            await Task.CompletedTask.Async();
        }

        [TestMethod]
        public async Task EventStream_Tests_NuGet_Test08()
        {
            var d = new D();
            var events = d.SubscribedTo().ToArray();
            Assert.AreEqual(2, events.Length);
            Assert.AreEqual(typeof(IE), events[0]);
            Assert.AreEqual(typeof(E2), events[1]);
            await Task.CompletedTask.Async();
        }

        public class A : ISubscribeTo<E>
        {
            public async Task On(E e) => await Task.CompletedTask;
        }

        public class B : ISubscribeTo<E>
        {
            public async Task On(E e) => await Task.CompletedTask;
        }

        public class C : ISubscribeTo<E2>
        {
            public async Task On(E2 e) => await Task.CompletedTask;
        }

        public class D : ISubscribeTo<IE>, ISubscribeTo<E2>
        {
            public async Task On(IE e) => await Task.CompletedTask;
            public async Task On(E2 e) => await Task.CompletedTask;
        }

        public interface IE { }
        public class E { }
        public class E1 : E, IE { }
        public class E2 : E { }
        public class E3 : E, IE { }

        private static EventStream ES(params ISubscribeTo[] subscribers) => new EventStream().Initialize(subscribers);
    }
}
