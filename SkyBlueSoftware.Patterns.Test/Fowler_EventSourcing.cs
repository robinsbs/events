﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.Events;
using SkyBlueSoftware.Events.Autofac;
using SkyBlueSoftware.TestFramework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Patterns.Test
{
    [TestClass]
    public class Fowler_EventSourcing
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public void Fowler_EventSourcing_Example()
        {
            var (events, calculator, log) = SkyBlueSoftwareEvents.Initialize(this, x => x.Is<IApp>(), x => x.Is<IAppNew>())
                                                                 .Resolve<IEventStream, Calculator, EventLog>();
            events.Publish<AddEvent>(2);
            events.Publish<AddEvent>(3);
            events.Publish<SubtractEvent>(1);
            t.Verify(new { calculator, log });
        }

        interface IApp { }
        interface IAppNew : IApp { }
        
        class AddEvent : IAppNew
        {
            public AddEvent(int value)
            {
                Value = value;
            }

            public int Value { get; }
        }

        class SubtractEvent : IAppNew
        {
            public SubtractEvent(int value)
            {
                Value = value;
            }

            public int Value { get; }
        }

        class Calculator : IApp, ISubscribeTo<AddEvent>, ISubscribeTo<SubtractEvent>
        {
            public int Value { get; private set; }
            public Task On(AddEvent e) => Task.FromResult(Value += e.Value);
            public Task On(SubtractEvent e) => Task.FromResult(Value -= e.Value);
        }

        class EventLog : IApp, ISubscribeTo<IApp>, IEnumerable<IApp>
        {
            private readonly IList<IApp> log;

            public EventLog()
            {
                log = new List<IApp>();
            }

            public Task On(IApp e) { log.Add(e); return Task.CompletedTask; }

            public IEnumerator<IApp> GetEnumerator() => log.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}