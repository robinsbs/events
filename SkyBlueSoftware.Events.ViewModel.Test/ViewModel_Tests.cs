using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.Events.Autofac;

namespace SkyBlueSoftware.Events.ViewModel.Test
{
    [TestClass]
    public class ViewModel_Tests
    {
        [TestMethod]
        public void ViewModel_Tests_Test01()
        {
            var app = SkyBlueSoftwareEvents.RegisterAllTypes<Main>()
                                               .Build()
                                               .InitializeEvents()
                                               .Resolve<Main>();
            Assert.IsNotNull(app);
        }
    }
}
