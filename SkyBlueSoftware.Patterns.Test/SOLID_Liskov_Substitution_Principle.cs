using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.TestFramework;
using System.Linq;

namespace SkyBlueSoftware.Patterns.Test
{
    [TestClass]
    public class SOLID_Liskov_Substitution_Principle
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public void SOLID_Liskov_Substitution_Principle_Example()
        {
            var b = new Base();
            Base d = new Derived_LSP_Compliant();
            Base d2 = new Derived_LSP_NonCompliant();
            var input = new[] { 1, 2, 3, 4, 5 };
            t.Verify(new { Base = b.DoWork(input), LSPCompliant = d.DoWork(input), LSPNonCompliant = d2.DoWork(input) });
        }

        class Base
        {
            public virtual int DoWork(params int[] numbers) => numbers.Sum();
        }

        class Derived_LSP_Compliant : Base
        {
            public override int DoWork(params int[] numbers) => numbers.Where(x => x % 2 == 0).Sum();
        }

        class Derived_LSP_NonCompliant : Base
        {
            public new int DoWork(params int[] numbers) => numbers.Where(x => x % 2 == 0).Sum();
        }
    }
}
