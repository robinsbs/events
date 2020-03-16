// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace SkyBlueSoftware.Storage
{
    public class SqliteDataProvider : DataProvider
    {
        private readonly string connectionString;

        public SqliteDataProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override IEnumerable<IDataRow> Execute(string command)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var dbCommand = connection.CreateCommand())
                {
                    dbCommand.CommandText = command;
                    using (var reader = dbCommand.ExecuteReader())
                    {
                        var dataReader = new DataReader(reader, CreateColumns(reader));
                        while (dataReader.Read())
                        {
                            yield return dataReader;
                        }
                    }
                }
            }
        }
    }
}