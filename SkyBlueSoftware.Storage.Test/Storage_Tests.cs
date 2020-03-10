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

            var r = new SqliteDataProvider(@"Data Source=..\..\..\sqlite.db").ExecuteReader("select * from document");
            while (r.Read())
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

            var r = new SqlServerDataProvider(@"Data Source=(local);Database=SBS;Integrated Security=true").ExecuteReader("select * from document");
            while (r.Read())
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

            IDataProvider dataProvider = new SqliteDataProvider(@"Data Source=..\..\..\sqlite.db");

            IDataReader r = dataProvider.ExecuteReader("select * from document");
            while (r.Read())
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

            IDataProvider dataProvider = new SqliteDataProvider(@"Data Source=..\..\..\sqlite.db");
            IDataReader r = dataProvider.ExecuteReader("select * from document");
            while (r.Read())
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

            IDataProvider dataProvider = new SqlServerDataProvider(@"Data Source=(local);Database=SBS;Integrated Security=true");

            IDataReader r = dataProvider.ExecuteReader("select * from document");
            while (r.Read())
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

            var r = dataProvider.ExecuteReader("select * from document");
            while (r.Read())
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

        public override IDataReader ExecuteReader(string command)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = command;
            var reader = dbCommand.ExecuteReader();
            return new DataReader(connection, dbCommand, reader, CreateColumns(reader));
        }
    }

    public class SqliteDataProvider : DataProvider
    {
        private readonly string connectionString;

        public SqliteDataProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override IDataReader ExecuteReader(string command)
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = command;
            var reader = dbCommand.ExecuteReader();
            return new DataReader(connection, dbCommand, reader, CreateColumns(reader));
        }
    }

    public interface IDataReader : IDisposable
    {
        bool Read();
        T GetValue<T>(int ordinal);
        T GetValue<T>(string name);
    }

    public class DataReader : IDataReader
    {
        private readonly IDisposable connection;
        private readonly IDisposable dbCommand;
        private readonly DbDataReader reader;
        private readonly ILookup<string, int> columns;

        public DataReader(IDisposable connection, IDisposable dbCommand, DbDataReader reader, ILookup<string, int> columns)
        {
            this.connection = connection;
            this.dbCommand = dbCommand;
            this.reader = reader;
            this.columns = columns;
        }

        public bool Read() => reader.Read();
        public T GetValue<T>(int ordinal) => reader.GetFieldValue<T>(ordinal);
        public T GetValue<T>(string name) => GetValue<T>(columns[name]);

        public void Dispose()
        {
            try
            {
                reader.Dispose();
            }
            finally
            {
                try
                {
                    dbCommand.Dispose();
                }
                finally
                {
                    connection.Dispose();
                }
            }
        }
    }

    public interface IDataProvider
    {
        IDataReader ExecuteReader(string command);
    }

    public abstract class DataProvider : IDataProvider
    {
        public abstract IDataReader ExecuteReader(string command);

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
