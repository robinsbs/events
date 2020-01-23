using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Storage.Test
{
    [TestClass]
    public class Storage_Tests
    {
        [TestMethod]
        public async Task Storage_Tests_Test01()
        {
            await foreach(var r in ReadAsync())
            {
                Console.WriteLine(r);
            }
        }

        private async IAsyncEnumerable<IRecord> ReadAsync()
        {
            var source = new CancellationTokenSource();
            var token = source.Token;
            using (var connection = new SqliteConnection(@"Data Source=..\..\..\sqlite.db"))
            {
                await connection.OpenAsync(token);
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from document";
                    var reader = await command.ExecuteReaderAsync(token);
                    while (await reader.ReadAsync(token))
                    {
                        yield return new Record();
                    }
                }
            }
        }

        public class Record : IRecord
        {

        }
        public interface IRecord
        {

        }
    }
}
