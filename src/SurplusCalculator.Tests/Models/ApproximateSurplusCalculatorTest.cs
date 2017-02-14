using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SurplusCalculator.Models;

namespace SurplusCalculator.Tests.Models
{
    [TestFixture(TestOf = typeof(ApproximateSurplusCalculator))]
    public class ApproximateSurplusCalculatorTest : SurplusCalculatorTestBase
    {
        protected override ISurplusCalculator CreateCalulator()
        {
            return new ApproximateSurplusCalculator();
        }

        [Test]
        public void CanCalculate6()
        {
            var surplusCalculator = CreateCalulator();
            var targetItemCountsByLengths = new Dictionary<int, int>
            {
                [4] = 3,
                [2] = 3,
            };
            var itemInfos = surplusCalculator.Calculate(6, targetItemCountsByLengths);

            Assert.That(() => itemInfos.Count, Is.EqualTo(3));
            Assert.That(() => itemInfos.Select(ItemInfoHelper.GetFreeLength).Sum(), Is.EqualTo(0));
        }

        [Test]
        public void CanCalculate7()
        {
            var surplusCalculator = CreateCalulator();
            var targetItemCountsByLengths = new Dictionary<int, int>
            {
                [5] = 3,
                [1] = 9,
            };
            var itemInfos = surplusCalculator.Calculate(8, targetItemCountsByLengths);

            Assert.That(() => itemInfos.Count, Is.EqualTo(3));
            Assert.That(() => itemInfos.Select(ItemInfoHelper.GetFreeLength).Sum(), Is.EqualTo(0));
        }

        [Test]
        public void CanCalculate8()
        {
            var surplusCalculator = CreateCalulator();
            var targetItemCountsByLengths = new Dictionary<int, int>
            {
                [7] = 5,
                [2] = 10,
            };
            var itemInfos = surplusCalculator.Calculate(10, targetItemCountsByLengths);

            Assert.That(() => itemInfos.Count, Is.EqualTo(6));
            Assert.That(() => itemInfos.Select(ItemInfoHelper.GetFreeLength).Sum(), Is.EqualTo(5));
        }
    }
}
