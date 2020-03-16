// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
}
