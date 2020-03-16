// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.TestFramework;

namespace SkyBlueSoftware.Framework.Test
{
    [TestClass]
    public class LookupCollection_Tests
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public void LookupCollection_Tests_Test01()
        {
            var c = new LookupCollection<int, string>(new Dictionary<int, string>(), x => string.Empty);
            for (var i = 0; i < 10; i++)
            {
                c.Add(i, $"value: {i}");
            }

            var results = new List<string>();

            for (int i = -10; i < 20; i++)
            {
                results.Add($"result {i}: {c[i]}");
            }

            t.Verify(results);
        }
    }
}
