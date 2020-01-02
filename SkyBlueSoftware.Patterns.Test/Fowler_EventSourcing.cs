using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.TestFramework;

namespace SkyBlueSoftware.Patterns.Test
{
    [TestClass]
    public class Fowler_EventSourcing
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public void Fowler_EventSourcing_Example()
        {
        }
    }
}
