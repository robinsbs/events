using System;
using System.Collections.Generic;

namespace SkyBlueSoftware.Framework
{
    public class LookupCollection<TKey, TValue> : ILookupCollection<TKey, TValue> where TKey : notnull where TValue : notnull
    {
        private readonly IDictionary<TKey, TValue> collection;
        private readonly Func<TKey, TValue> defaultValue;

        public LookupCollection(IDictionary<TKey, TValue> collection, Func<TKey, TValue> defaultValue)
        {
            this.collection = collection;
            this.defaultValue = defaultValue;
        }

        public TValue this[TKey key]
        {
            get => collection.TryGetValue(key, out var value) ? value : defaultValue(key);
            set => Add(key, value);
        }

        public TValue Add(TKey key, TValue value)
        {
            collection.Add(key, value);
            return value;
        }
    }

    public static class LookupCollection
    {
        public static ILookup<TKey, TValue> CreateLookup<TKey, TValue>(Func<TKey, TValue> defaultValue) where TKey : notnull where TValue : notnull
        {
            return new LookupCollection<TKey, TValue>(new Dictionary<TKey, TValue>(), defaultValue);
        }

        public static ILookupCollection<TKey, TValue> CreateLookupCollection<TKey, TValue>(Func<TKey, TValue> defaultValue, int capacity = 0, IEqualityComparer<TKey>? comparer = null) where TKey : notnull where TValue : notnull
        {
            return new LookupCollection<TKey, TValue>(new Dictionary<TKey, TValue>(), defaultValue);
        }
    }
}