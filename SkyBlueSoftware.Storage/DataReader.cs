// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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