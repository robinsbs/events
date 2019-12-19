using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.TestFramework;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.Test.NuGet
{
    [TestClass]
    public class EventStream_Tests_NuGet
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public async Task EventStream_Tests_NuGet_Test01()
        {
        }
    }
}
