using System;
using System.Collections.Generic;
using System.Data.Common;
using SkyBlueSoftware.Framework;

namespace SkyBlueSoftware.Storage
{
    public abstract class DataProvider : IDataProvider
    {
        public abstract IEnumerable<IDataRow> Execute(string command);

        protected ILookup<string, int> CreateColumns(DbDataReader reader)
        {
            var fieldCount = reader.FieldCount;
            var columns = LookupCollection.CreateLookupCollection<string, int>(x => -1, fieldCount, StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < fieldCount; i++)
            {
                columns[reader.GetName(i)] = i;
            }
            return columns;
        }
    }
}