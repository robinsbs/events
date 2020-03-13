using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace SkyBlueSoftware.Storage
{
    public class SqlServerDataProvider : DataProvider
    {
        private readonly string connectionString;

        public SqlServerDataProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override IEnumerable<IDataRow> Execute(string command)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = command;
            var reader = dbCommand.ExecuteReader();
            var dataReader = new DataReader(reader, CreateColumns(reader));
            while (dataReader.Read())
            {
                yield return dataReader;
            }
        }
    }
}