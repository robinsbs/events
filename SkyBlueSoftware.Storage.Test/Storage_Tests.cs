// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
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
        public void Storage_Tests_Sqlite_Select_Columns()
        {
            T(Sqlite(), "select * from document", Columns);
        }

        [TestMethod, ExpectedException(typeof(SqliteException))]
        public void Storage_Tests_Sqlite_Proc_Columns()
        {
            T(Sqlite(), "DocumentLoad", Columns);
        }

        [TestMethod]
        public void Storage_Tests_Sqlite_Select_Ordinals()
        {
            T(Sqlite(), "select * from document", Ordinals);
        }

#if !IsBuildServer
        [TestMethod]
        public void Storage_Tests_SqlServer_Proc_Columns()
        {
            T(SqlServer(), "DocumentLoad", Columns);
        }

        [TestMethod]
        public void Storage_Tests_SqlServer_Proc_Ordinals()
        {
            T(SqlServer(), "DocumentLoad", Ordinals);
        }

        [TestMethod]
        public void Storage_Tests_SqlServer_Select_Columns()
        {
            T(SqlServer(), "select * from document", Columns);
        }

        [TestMethod]
        public void Storage_Tests_SqlServer_Select_Ordinals()
        {
            T(SqlServer(), "select * from document", Ordinals);
        }
#endif

        private static SqlServerDataProvider SqlServer()
        {
            return new SqlServerDataProvider(@"Data Source=(local);Database=SBS;Integrated Security=true");
        }

        private static SqliteDataProvider Sqlite()
        {
            return new SqliteDataProvider(@"Data Source=..\..\..\sqlite.db");
        }

        private void T(IDataProvider dataProvider, string command, Func<IDataRow, string> rowSelector)
        {
            var results = new List<string>();

            var rows = dataProvider.Execute(command);
            foreach (var r in rows)
            {
                results.Add(rowSelector(r));
            }

            t.Verify(results);
        }

        private string Columns(IDataRow r)
        {
            var id = r.GetValue<int>("Id");
            var date = r.GetValue<DateTime>("Date");
            var text = r.GetValue<string>("Text");
            return $"{id};{date.MDYHH()};{text}";
        }

        private string Ordinals(IDataRow r)
        {
            var id = r.GetValue<int>(0);
            var date = r.GetValue<DateTime>(1);
            var text = r.GetValue<string>(2);
            return $"{id};{date.MDYHH()};{text}";
        }
    }
}
