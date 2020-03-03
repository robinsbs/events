using System;
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

    public class LookupCollection<TKey, TValue> where TKey : notnull where TValue : notnull
    {
        private readonly IDictionary<TKey, TValue> collection;
        private readonly Func<TKey, TValue> defaultValue;

        public LookupCollection(IDictionary<TKey, TValue> collection, Func<TKey, TValue> defaultValue)
        {
            this.collection = collection;
            this.defaultValue = defaultValue;
        }

        public TValue this[TKey key] => collection.TryGetValue(key, out var value) ? value : defaultValue(key);
        public void Add(TKey key, TValue value) => collection.Add(key, value);
    }
}
