// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.TestFramework;
using System.Collections.Generic;

namespace SkyBlueSoftware.Patterns.Test
{
    [TestClass]
    public class GOF_Facade
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public void GOF_Facade_Example()
        {
            var f = new Facade(new SubSystemA(), new SubSystemB(), new SubSystemC());
            t.Verify(f.Execute());
        }

        private class Facade
        {
            private readonly SubSystemA a;
            private readonly SubSystemB b;
            private readonly SubSystemC c;

            public Facade(SubSystemA a, SubSystemB b, SubSystemC c)
            {
                this.a = a;
                this.b = b;
                this.c = c;
            }

            public IEnumerable<string> Execute()
            {
                yield return a.Execute();
                yield return b.Run();
                yield return c.Log();
            }
        }

        private class SubSystemA
        {
            public string Execute() => "Hello world from SubSystemA.";
        }

        private class SubSystemB
        {
            public string Run() => "Hello world from SubSystemB.";
        }

        private class SubSystemC
        {
            public string Log() => "Hello world from SubSystemC.";
        }
    }
}
