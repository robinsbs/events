using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SkyBlueSoftware.Events.Test
{
    [TestClass]
    public class EventStream_Instantiation_Tests
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public async Task EventStream_Instantiation_Tests_Test01()
        {
            var b = new B();
            var events = new EventStream(new[] { b });
            var a = new A(events);
            await a.Select();
            t.Verify(b);
        }

        public class E { }

        public class A 
        {
            private readonly IEventStream events;

            public A(IEventStream events)
            {
                this.events = events;
            }

            public async Task Select() => await events.Publish(new E());
        }
        
        public class B : ISubscribeTo<E>
        {
            public B()
            {
                Value = string.Empty;
            }

            public string Value { get; private set; }
            public async Task On(E e)
            {
                Value = $"{GetType().Name} received {e?.GetType().Name}";
                await Task.CompletedTask;
            }
        }
    }
}
