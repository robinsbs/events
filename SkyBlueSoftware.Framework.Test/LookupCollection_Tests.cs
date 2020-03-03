using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.TestFramework;

namespace SkyBlueSoftware.Framework.Test
{
    [TestClass]
    public class LookupCollection_Tests
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public void LookupCollection_Tests_Test01()
        {
            t.Verify(new { Result = "It's working." });
        }
    }
}
