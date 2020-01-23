using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Storage.Test
{
    [TestClass]
    public class Storage_Tests
    {
        [TestMethod]
        public async Task Storage_Tests_Sqlite()
        {
            await foreach(var r in ReadAsync(() => new SqliteConnection(@"Data Source=..\..\..\sqlite.db")))
            {
                var id = await r.GetValueAsync<int>("Id");
                var date = await r.GetValueAsync<DateTime>("Date");
                var text = await r.GetValueAsync<string>("Text");
                Console.WriteLine($"{id};{date};{text}");
            }
        }

#if !IsBuildServer
        [TestMethod]
        public async Task Storage_Tests_SqlServer()
        {
            await foreach (var r in ReadAsync(() => new SqlConnection(@"Data Source=(local);Database=SBS;Integrated Security=true")))
            {
                var id = await r.GetValueAsync<int>("Id");
                var date = await r.GetValueAsync<DateTime>("Date");
                var text = await r.GetValueAsync<string>("Text");
                Console.WriteLine($"{id};{date};{text}");
            }
        }
#endif

        private async IAsyncEnumerable<IRecord> ReadAsync(Func<DbConnection> connectionFactory)
        {
            var source = new CancellationTokenSource();
            var token = source.Token;
            using (var connection = connectionFactory())
            {
                await connection.OpenAsync(token);
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from document";
                    var reader = await command.ExecuteReaderAsync(token);
                    var columns = CreateColumns(reader);
                    while (await reader.ReadAsync(token))
                    {
                        yield return new Record(reader, columns);
                    }
                }
            }
        }

        private IDictionary<string, int> CreateColumns(DbDataReader reader)
        {
            var fieldCount = reader.FieldCount;
            var columns = new Dictionary<string, int>(fieldCount, StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < fieldCount; i++)
            {
                columns[reader.GetName(i)] = i;
            }
            return columns;
        }

        public class Record : IRecord
        {
            private readonly DbDataReader reader;
            private readonly IDictionary<string, int> columns;

            public Record(DbDataReader reader, IDictionary<string, int> columns)
            {
                this.reader = reader;
                this.columns = columns;
            }

            public async Task<T> GetValueAsync<T>(int ordinal)
            {
                return await reader.GetFieldValueAsync<T>(ordinal);
            }

            public async Task<T> GetValueAsync<T>(string name)
            {
                return await reader.GetFieldValueAsync<T>(columns[name]);
            }
        }

        public interface IRecord
        {
            public Task<T> GetValueAsync<T>(int ordinal);
            public Task<T> GetValueAsync<T>(string name);
        }
    }
}
