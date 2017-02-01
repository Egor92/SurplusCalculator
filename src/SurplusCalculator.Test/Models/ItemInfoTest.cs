using System.Linq;
using NUnit.Framework;
using SurplusCalculator.Models;

namespace SurplusCalculator.Test.Models
{
    [TestFixture]
    public class ItemInfoTest
    {
        [Test]
        public void IfHasTergetItems_CanCalculateSurplus()
        {
            var sourceItemLenght = 60;
            var targetItemLenghts = new []{10, 8, 14, 6.5, 1.2, 4.4, 3};
            var outputDataItem = new ItemInfo(sourceItemLenght, targetItemLenghts);

            var actualSurplus = outputDataItem.GetSurplus();
            var expectedSurplus = sourceItemLenght - targetItemLenghts.Sum();

            Assert.That(() => actualSurplus, Is.EqualTo(expectedSurplus));
        }

        [Test]
        public void IfHasNoTergetItems_CanCalculateSurplus()
        {
            var sourceItemLenght = 60;
            var targetItemLenghts = new double[0];
            var outputDataItem = new ItemInfo(sourceItemLenght, targetItemLenghts);

            var actualSurplus = outputDataItem.GetSurplus();

            Assert.That(() => actualSurplus, Is.EqualTo(sourceItemLenght));
        }
    }
}
