using NUnit.Framework;
using SurplusCalculator.Models;
using Calculator = SurplusCalculator.Models.SurplusCalculator;

namespace SurplusCalculator.Tests.Models
{
    [TestFixture(TestOf = typeof(Calculator))]
    public class SurplusCalculatorTest : SurplusCalculatorTestBase
    {
        protected override ISurplusCalculator CreateCalulator()
        {
            return new Calculator();
        }
    }
}
