using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SkyBlueSoftware.Framework;
using SkyBlueSoftware.TestFramework;

namespace SkyBlueSoftware.Storage.Test
{
    [TestClass]
    public class Storage_Tests
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        /// <summary>
        /// TODO:
        /// 1. cancellation
        /// 2. stored procedures
        /// 3. multiple results
        /// 4. json results
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Storage_Tests_Sqlite()
        {
            var results = new List<string>();

            foreach(var r in Read(() => new SqliteConnection(@"Data Source=..\..\..\sqlite.db"), "select * from document"))
            {
                var id = r.GetValue<int>("Id");
                var date = r.GetValue<DateTime>("Date");
                var text = r.GetValue<string>("Text");
                results.Add($"{id};{date.MDYHH()};{text}");
            }

            t.Verify(results);
        }

        [TestMethod]
        public async Task Storage_Tests_Sqlite_Async()
        {
            var results = new List<string>();

            await foreach(var r in ReadAsync(() => new SqliteConnection(@"Data Source=..\..\..\sqlite.db"), "select * from document"))
            {
                var id = await r.GetValueAsync<int>("Id");
                var date = await r.GetValueAsync<DateTime>("Date");
                var text = await r.GetValueAsync<string>("Text");
                results.Add($"{id};{date.MDYHH()};{text}");
            }

            t.Verify(results);
        }

#if !IsBuildServer
        [TestMethod]
        public async Task Storage_Tests_SqlServer()
        {
            var results = new List<string>();

            await foreach (var r in ReadAsync(() => new SqlConnection(@"Data Source=(local);Database=SBS;Integrated Security=true"), "select * from document"))
            {
                var id = await r.GetValueAsync<int>("Id");
                var date = await r.GetValueAsync<DateTime>("Date");
                var text = await r.GetValueAsync<string>("Text");
                results.Add($"{id};{date.MDYHH()};{text}");
            }

            t.Verify(results, nameof(Storage_Tests_SqlServer));
        }
#endif

        private IEnumerable<IRecord> Read(Func<DbConnection> connectionFactory, string command)
        {
            using var connection = connectionFactory();
            connection.Open();
            using var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = command;
            var reader = dbCommand.ExecuteReader();
            var columns = CreateColumns(reader);
            while (reader.Read())
            {
                yield return new Record(reader, columns, CancellationToken.None);
            }
        }

        private async IAsyncEnumerable<IRecord> ReadAsync(Func<DbConnection> connectionFactory, string command)
        {
            var source = new CancellationTokenSource();
            var token = source.Token;
            await using var connection = connectionFactory();
            await connection.OpenAsync(token);
            await using var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = command;
            var reader = await dbCommand.ExecuteReaderAsync(token);
            var columns = CreateColumns(reader);
            while (await reader.ReadAsync(token))
            {
                yield return new Record(reader, columns, token);
            }
        }

        private ILookup<string, int> CreateColumns(DbDataReader reader)
        {
            var fieldCount = reader.FieldCount;
            var columns = LookupCollection.CreateLookupCollection<string, int>(x => -1, fieldCount, StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < fieldCount; i++)
            {
                columns[reader.GetName(i)] = i;
            }
            return columns;
        }

        public class Record : IRecord
        {
            private readonly DbDataReader reader;
            private readonly ILookup<string, int> columns;
            private readonly CancellationToken token;

            public Record(DbDataReader reader, ILookup<string, int> columns, CancellationToken token)
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
