namespace SkyBlueSoftware.Framework
{
    public interface ILookupCollection<in TKey, TValue> : ILookup<TKey, TValue>
    {
        new TValue this[TKey key] { get; set; }
        TValue Add(TKey key, TValue value);
    }
}