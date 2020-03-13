namespace SkyBlueSoftware.Storage
{
    public interface IDataRow
    {
        T GetValue<T>(int ordinal);
        T GetValue<T>(string name);
    }
}