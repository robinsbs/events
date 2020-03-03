using System;
using System.Collections.Generic;

namespace SkyBlueSoftware.Framework
{
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