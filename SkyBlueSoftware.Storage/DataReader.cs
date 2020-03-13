using System.Data.Common;
using SkyBlueSoftware.Framework;

namespace SkyBlueSoftware.Storage
{
    public class DataReader : IDataRow
    {
        private readonly DbDataReader reader;
        private readonly ILookup<string, int> columns;

        public DataReader(DbDataReader reader, ILookup<string, int> columns)
        {
            this.reader = reader;
            this.columns = columns;
        }

        public bool Read() => reader.Read();
        public T GetValue<T>(int ordinal) => reader.GetFieldValue<T>(ordinal);
        public T GetValue<T>(string name) => GetValue<T>(columns[name]);
    }
}