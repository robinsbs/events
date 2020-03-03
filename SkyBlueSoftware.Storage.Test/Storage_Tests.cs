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
        /// <summary>
        /// TODO:
        /// 1. cancellation
        /// 2. stored procedures
        /// 3. multiple results
        /// 4. json results
        /// </summary>
        /// <returns></returns>
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
            await using var connection = connectionFactory();
            await connection.OpenAsync(token);
            await using var command = connection.CreateCommand();
            command.CommandText = "select * from document";
            var reader = await command.ExecuteReaderAsync(token);
            var columns = CreateColumns(reader);
            while (await reader.ReadAsync(token))
            {
                yield return new Record(reader, columns, token);
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
            private readonly CancellationToken token;

            public Record(DbDataReader reader, IDictionary<string, int> columns, CancellationToken token)
            {
                this.reader = reader;
                this.columns = columns;
                this.token = token;
            }

            public T GetValue<T>(int ordinal) => reader.GetFieldValue<T>(ordinal);
            public T GetValue<T>(string name) => GetValue<T>(columns[name]);
            public Task<T> GetValueAsync<T>(int ordinal) => reader.GetFieldValueAsync<T>(ordinal, token);
            public Task<T> GetValueAsync<T>(string name) => GetValueAsync<T>(columns[name]);
        }

        public interface IRecord
        {
            // column name
            // column ordinal
            // columns
            // get values
            // isdbnull
            public T GetValue<T>(int ordinal);
            public T GetValue<T>(string name);
            public Task<T> GetValueAsync<T>(int ordinal);
            public Task<T> GetValueAsync<T>(string name);
        }
    }
}
