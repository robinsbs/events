using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.Events.Autofac;
using SkyBlueSoftware.TestFramework;

namespace SkyBlueSoftware.Events.ViewModel.Test
{
    [TestClass]
    public class ViewModel_Tests
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public void ViewModel_Tests_Test01()
        {
            var app = SkyBlueSoftwareEvents.InitializeApp<Main>();
            t.Verify(app);
        }
    }
}
