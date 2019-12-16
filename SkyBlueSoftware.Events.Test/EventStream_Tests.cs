using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SkyBlueSoftware.Events.Test
{
    [TestClass]
    public class EventStream_Tests
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public async Task EventStream_Tests_Test01()
        {
            var events = E(new A(), new B());
            await events.Publish(new E1());
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test02()
        {
            var events = E(new A(), new B());
            events.Where(x => x.Subscriber == typeof(A)).ForEach(x => x.Unsubscribe());
            await events.Publish(new E());
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test03()
        {
            var events = E(new C());
            await events.Publish(new E2());
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test04()
        {
            var events = E(new C());
            await events.Publish(new E());
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test05()
        {
            var events = E(new D());
            await events.Publish(new E1());
            await events.Publish(new E2());
            await events.Publish(new E3());
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test06()
        {
            var events = E(new A(), new B());
            events.Where(x => x.Subscriber == typeof(A)).FirstOrDefault().Unsubscribe().Resubscribe();
            await events.Publish(new E());
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test07()
        {
            var a = new A();
            var events = a.SubscribedTo().ToArray();
            Assert.AreEqual(1, events.Length);
            Assert.AreEqual(typeof(E), events[0]);
            await Task.CompletedTask.Async();
        }

        [TestMethod]
        public async Task EventStream_Tests_Test08()
        {
            var d = new D();
            var events = d.SubscribedTo().ToArray();
            Assert.AreEqual(2, events.Length);
            Assert.AreEqual(typeof(IE), events[0]);
            Assert.AreEqual(typeof(E2), events[1]);
            await Task.CompletedTask.Async();
        }

        private static EventStream E(params ISubscribeTo[] subscribers) => new EventStream(subscribers);
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
}
