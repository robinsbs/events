// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace SkyBlueSoftware.Framework
{
    public interface ILookupCollection<in TKey, TValue> : ILookup<TKey, TValue>
    {
        new TValue this[TKey key] { get; set; }
        TValue Add(TKey key, TValue value);
    }
}