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
                var id = await r.GetValueAsync<int>(0);
                var date = await r.GetValueAsync<DateTime>(1);
                var text = await r.GetValueAsync<string>(2);
                Console.WriteLine($"{id};{date};{text}");
            }
        }

#if !IsBuildServer
        [TestMethod]
        public async Task Storage_Tests_SqlServer()
        {
            await foreach (var r in ReadAsync(() => new SqlConnection(@"Data Source=(local);Database=SBS;Integrated Security=true")))
            {
                var id = await r.GetValueAsync<int>(0);
                var date = await r.GetValueAsync<DateTime>(1);
                var text = await r.GetValueAsync<string>(2);
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
                    while (await reader.ReadAsync(token))
                    {
                        yield return new Record(reader);
                    }
                }
            }
        }

        public class Record : IRecord
        {
            private readonly DbDataReader reader;

            public Record(DbDataReader reader)
            {
                this.reader = reader;
            }

            public async Task<T> GetValueAsync<T>(int ordinal)
            {
                return await reader.GetFieldValueAsync<T>(ordinal);
            }
        }

        public interface IRecord
        {
            public Task<T> GetValueAsync<T>(int ordinal);
        }
    }
}
