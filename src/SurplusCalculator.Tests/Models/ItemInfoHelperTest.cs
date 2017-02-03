using System.Linq;
using NUnit.Framework;
using SurplusCalculator.Models;

namespace SurplusCalculator.Tests.Models
{
    [TestFixture]
    public class ItemInfoHelperTest
    {
        [Test]
        public void IfHasTargetItems_CanCalculateFreeLength()
        {
            var sourceItemLenght = 60;
            var targetItemLenghts = new[]
            {
                10, 
                8, 
                14, 
                6, 
                1, 
                4, 
                3
            };
            var outputDataItem = new ItemInfo(sourceItemLenght, targetItemLenghts);

            var actualSurplus = ItemInfoHelper.GetFreeLength(outputDataItem);
            var expectedSurplus = sourceItemLenght - targetItemLenghts.Sum();

            Assert.That(() => actualSurplus, Is.EqualTo(expectedSurplus));
        }

        [Test]
        public void IfHasNoTargetItems_CanCalculateFreeLength()
        {
            var sourceItemLenght = 60;
            var targetItemLenghts = new int[0];
            var outputDataItem = new ItemInfo(sourceItemLenght, targetItemLenghts);

            var actualSurplus = ItemInfoHelper.GetFreeLength(outputDataItem);

            Assert.That(() => actualSurplus, Is.EqualTo(sourceItemLenght));
        }

        [Test]
        public void IfItemInfoIsEmpty_ThenHashWillEpmty()
        {
            var itemInfo = new ItemInfo(0, new int[0]);
            var hash = ItemInfoHelper.GetHash(itemInfo);

            Assert.That(hash, Is.EqualTo("{}"));
        }

        [Test]
        public void IfItemInfoIsNotEmpty_ThenHashWillEnumerateOrderedLengths()
        {
            var targetItemLenghts = new[]
            {
                5, 
                4, 
                9, 
                1, 
                3, 
                2, 
                5, 
                6, 
                7, 
            };
            var itemInfo = new ItemInfo(0, targetItemLenghts);
            var hash = ItemInfoHelper.GetHash(itemInfo);

            var orderedLengths = targetItemLenghts.OrderByDescending(x => x);
            var expectedHash = $"{{{string.Join(";", orderedLengths)}}}";
            Assert.That(hash, Is.EqualTo(expectedHash));
        }

        [Test]
        public void IfHasNoItemInfos_ThenHasWillEmpty()
        {
            var itemInfos = new ItemInfo[0];
            var hash = ItemInfoHelper.GetHash(itemInfos);

            Assert.That(hash, Is.EqualTo(string.Empty));
        }

        [Test]
        public void IsHasItemInfos_ThenHashWillEnumerateOrderedItemsInfoHashed()
        {
            var itemInfos = new[]
            {
                new ItemInfo(0, new[]
                {
                    6, 
                    1, 
                    9, 
                    7, 
                    3
                }), 
                new ItemInfo(0, new[]
                {
                    1, 
                    9, 
                    3, 
                    4
                }), 
                new ItemInfo(0, new[]
                {
                    9, 
                    4, 
                    3, 
                    2, 
                    6, 
                    7
                }), 
            };
            var hash = ItemInfoHelper.GetHash(itemInfos);

            Assert.That(hash, Is.EqualTo("{9;7;6;4;3;2}{9;7;6;3;1}{9;4;3;1}"));
        }
    }
}
