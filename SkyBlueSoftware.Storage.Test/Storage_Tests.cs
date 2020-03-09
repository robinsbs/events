using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

#if !IsBuildServer
        [TestMethod]
        public void Storage_Tests_SqlServer()
        {
            var results = new List<string>();

            foreach (var r in Read(() => new SqlConnection(@"Data Source=(local);Database=SBS;Integrated Security=true"), "select * from document"))
            {
                var id = r.GetValue<int>("Id");
                var date = r.GetValue<DateTime>("Date");
                var text = r.GetValue<string>("Text");
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
                yield return new Record(reader, columns);
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

        [TestMethod]
        public void Storage_Tests_SqliteDataProvider_Columns()
        {
            var results = new List<string>();

            var dataProvider = new SqliteDataProvider();

            foreach (var r in dataProvider.Execute("select * from document"))
            {
                var id = r.GetValue<int>("Id");
                var date = r.GetValue<DateTime>("Date");
                var text = r.GetValue<string>("Text");
                results.Add($"{id};{date.MDYHH()};{text}");
            }

            t.Verify(results);
        }

        [TestMethod]
        public void Storage_Tests_SqliteDataProvider_Ordinals()
        {
            var results = new List<string>();

            var dataProvider = new SqliteDataProvider();

            foreach (var r in dataProvider.Execute("select * from document"))
            {
                var id = r.GetValue<int>(0);
                var date = r.GetValue<DateTime>(1);
                var text = r.GetValue<string>(2);
                results.Add($"{id};{date.MDYHH()};{text}");
            }

            t.Verify(results);
        }

#if !IsBuildServer
        [TestMethod]
        public void Storage_Tests_SqlServerDataProvider_Ordinals()
        {
            var results = new List<string>();

            var dataProvider = new SqlServerDataProvider();

            foreach (var r in dataProvider.Execute("select * from document"))
            {
                var id = r.GetValue<int>(0);
                var date = r.GetValue<DateTime>(1);
                var text = r.GetValue<string>(2);
                results.Add($"{id};{date.MDYHH()};{text}");
            }

            t.Verify(results);
        }

        [TestMethod]
        public void Storage_Tests_SqlServerDataProvider_Columns()
        {
            var results = new List<string>();

            var dataProvider = new SqlServerDataProvider();

            foreach (var r in dataProvider.Execute("select * from document"))
            {
                var id = r.GetValue<int>("Id");
                var date = r.GetValue<DateTime>("Date");
                var text = r.GetValue<string>("Text");
                results.Add($"{id};{date.MDYHH()};{text}");
            }

            t.Verify(results);
        }
#endif

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
    }

    public class Record : IRecord
    {
        private readonly DbDataReader reader;
        private readonly ILookup<string, int> columns;

        public Record(DbDataReader reader, ILookup<string, int> columns)
        {
            this.reader = reader;
            this.columns = columns;
        }

        public T GetValue<T>(int ordinal) => reader.GetFieldValue<T>(ordinal);
        public T GetValue<T>(string name) => GetValue<T>(columns[name]);
    }

    public class SqlServerDataProvider : DataProvider<SqlConnection>
    {
        public override IEnumerable<IRecord> Execute(string command)
        {
            using var connection = CreateConnection();
            connection.Open();
            using var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = command;
            using var reader = dbCommand.ExecuteReader();
            while (reader.Read())
            {
                yield return new Record(reader, CreateColumns(reader));
            }
        }

        protected override SqlConnection CreateConnection() => new SqlConnection(@"Data Source=(local);Database=SBS;Integrated Security=true");
    }

    public class SqliteDataProvider : DataProvider<SqliteConnection>
    {
        public override IEnumerable<IRecord> Execute(string command)
        {
            using var connection = CreateConnection();
            connection.Open();
            using var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = command;
            using var reader = dbCommand.ExecuteReader();
            while (reader.Read())
            {
                yield return new Record(reader, CreateColumns(reader));
            }
        }

        protected override SqliteConnection CreateConnection() => new SqliteConnection(@"Data Source=..\..\..\sqlite.db");
    }

    public interface IDataProvider
    {
        IEnumerable<IRecord> Execute(string command);
    }

    public abstract class DataProvider<T> : IDataProvider where T : IDbConnection
    {
        public abstract IEnumerable<IRecord> Execute(string command);

        protected abstract T CreateConnection();

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
