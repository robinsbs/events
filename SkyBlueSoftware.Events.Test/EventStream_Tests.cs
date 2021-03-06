// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.Events.Autofac;
using SkyBlueSoftware.TestFramework;

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
            var events = CE(typeof(A), typeof(B), typeof(E1));
            await events.Publish<E1>().Async();
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test02()
        {
            var events = CE(typeof(A), typeof(B), typeof(E));
            events.Where(x => x.Subscriber == typeof(A)).ForEach(x => x.Unsubscribe());
            await events.Publish<E>().Async();
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test03()
        {
            var events = CE(typeof(C), typeof(E2));
            await events.Publish<E2>().Async();
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test04()
        {
            var events = CE(typeof(C), typeof(E));
            await events.Publish<E>().Async();
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test05()
        {
            var events = CE(typeof(D), typeof(E1), typeof(E2), typeof(E3));
            await events.Publish<E1>().Async();
            await events.Publish<E2>().Async();
            await events.Publish<E3>().Async();
            t.Verify(events);
        }

        [TestMethod]
        public async Task EventStream_Tests_Test06()
        {
            var events = CE(typeof(A), typeof(B), typeof(E));
            events.FirstOrDefault(x => x.Subscriber == typeof(A)).Unsubscribe().Resubscribe();
            await events.Publish<E>().Async();
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

        public class A : ISubscribeTo<E>, IRequireRegistration
        {
            public async Task On(E e) => await Task.CompletedTask.Async();
        }

        public class B : ISubscribeTo<E>, IRequireRegistration
        {
            public async Task On(E e) => await Task.CompletedTask.Async();
        }

        public class C : ISubscribeTo<E2>, IRequireRegistration
        {
            public async Task On(E2 e) => await Task.CompletedTask.Async();
        }

        public class D : ISubscribeTo<IE>, ISubscribeTo<E2>, IRequireRegistration
        {
            public async Task On(IE e) => await Task.CompletedTask.Async();
            public async Task On(E2 e) => await Task.CompletedTask.Async();
        }

        public interface IE { }
        public class E : IRequireRegistrationNew { }
        public class E1 : E, IE { }
        public class E2 : E { }
        public class E3 : E, IE { }

        public static IEventStream CE(params Type[] types)
        {
            return SkyBlueSoftwareEvents.RegisterAllTypes(x => x.Is<IRequireRegistration>(), x => x.Is<IRequireRegistrationNew>(), types)
                                        .Build()
                                        .InitializeEvents()
                                        .Resolve<IEventStream>();
        }
    }
}
