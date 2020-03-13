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