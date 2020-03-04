namespace SkyBlueSoftware.Framework
{
    public interface ILookup<in TKey, out TValue>
    {
        public TValue this[TKey key] { get; }
    }
}