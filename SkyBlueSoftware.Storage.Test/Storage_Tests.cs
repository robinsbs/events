using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var source = new CancellationTokenSource();
            var token = source.Token;
            using (var connection = new SqlConnection("Server=(local);Integrated Security=true;"))
            {
                await connection.OpenAsync(token);
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from sbs.dbo.document";
                    var reader = await command.ExecuteReaderAsync(token);
                    while (await reader.ReadAsync(token))
                    {
                    }
                }
            }
        }

        //private IAsyncEnumerable<IRecord> ReadAsync()
        //{

        //}
    }
}
