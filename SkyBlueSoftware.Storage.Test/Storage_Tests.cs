using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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

            var dataProvider = new SqliteDataProvider(@"Data Source=..\..\..\sqlite.db");
            var rows = dataProvider.Execute("select * from document");
            foreach (var r in rows)
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

            var dataProvider = new SqlServerDataProvider(@"Data Source=(local);Database=SBS;Integrated Security=true");
            var rows = dataProvider.Execute("select * from document");
            foreach (var r in rows)
            {
                var id = r.GetValue<int>("Id");
                var date = r.GetValue<DateTime>("Date");
                var text = r.GetValue<string>("Text");
                results.Add($"{id};{date.MDYHH()};{text}");
            }

            t.Verify(results, nameof(Storage_Tests_SqlServer));
        }
#endif

        [TestMethod]
        public void Storage_Tests_SqliteDataProvider_Columns()
        {
            var results = new List<string>();

            var dataProvider = new SqliteDataProvider(@"Data Source=..\..\..\sqlite.db");

            var rows = dataProvider.Execute("select * from document");
            foreach (var r in rows)
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

            var dataProvider = new SqliteDataProvider(@"Data Source=..\..\..\sqlite.db");
            var rows = dataProvider.Execute("select * from document");
            foreach (var r in rows)
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

            var dataProvider = new SqlServerDataProvider(@"Data Source=(local);Database=SBS;Integrated Security=true");

            var rows = dataProvider.Execute("select * from document");
            foreach (var r in rows)
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

            var dataProvider = new SqlServerDataProvider(@"Data Source=(local);Database=SBS;Integrated Security=true");

            var rows = dataProvider.Execute("select * from document");
            foreach (var r in rows)
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

    public class SqliteDataProvider : DataProvider
    {
        private readonly string connectionString;

        public SqliteDataProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override IEnumerable<IDataRow> Execute(string command)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = command;
            using var reader = dbCommand.ExecuteReader();
            var dataReader = new DataReader(reader, CreateColumns(reader));
            while (dataReader.Read())
            {
                yield return dataReader;
            }
        }
    }

    public interface IDataRow
    {
        T GetValue<T>(int ordinal);
        T GetValue<T>(string name);
    }

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

    public interface IDataProvider
    {
        IEnumerable<IDataRow> Execute(string command);
    }

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
