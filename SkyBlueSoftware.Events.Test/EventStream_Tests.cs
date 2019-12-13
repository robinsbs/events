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
            var events = EventStream.Create().Subscribe(new A(), new B());
            await events.Publish(new E1());
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test02()
        {
            var events = EventStream.Create();
            var subscriberA = events.Subscribe(new A()).FirstOrDefault();
            events.Subscribe(new B());
            subscriberA.Unsubscribe();
            await events.Publish(new E());
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test03()
        {
            var events = EventStream.Create().Subscribe(new C());
            await events.Publish(new E2());
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test04()
        {
            var events = EventStream.Create().Subscribe(new C());
            await events.Publish(new E());
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test05()
        {
            var events = EventStream.Create().Subscribe(new D());
            await events.Publish(new E1());
            await events.Publish(new E2());
            await events.Publish(new E3());
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test06()
        {
            var events = EventStream.Create();
            var subscriberA = events.Subscribe(new A()).FirstOrDefault();
            events.Subscribe(new B());
            subscriberA.Unsubscribe().Resubscribe();
            await events.Publish(new E());
            t.Verify(events);
        }
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
