using System;
using NUnit.Framework;
using SurplusCalculator.Models;
using Calculator = SurplusCalculator.Models.SurplusCalculator;

namespace SurplusCalculator.Tests.Models
{
    [TestFixture(TestOf = typeof(Calculator))]
    public class SurplusCalculatorTest
    {
        [Test]
        public void CanCreateInstance()
        {
            var surplusCalculator = new Calculator();
        }

        [Test]
        public void CanCalculate1()
        {
            var surplusCalculator = new Calculator();
            var targetItemLengths = new []
            {
                6,
                5,
                3,
            };
            var itemInfos = surplusCalculator.Calculate(6, targetItemLengths);

            var expected = new[]
            {
                new ItemInfo(6, new [] { 6 }),
                new ItemInfo(6, new [] { 5 }),
                new ItemInfo(6, new [] { 3 }),
            };
            CollectionAssert.AreEquivalent(expected, itemInfos);
        }

        [Test]
        public void CanCalculate2()
        {
            var surplusCalculator = new Calculator();
            var targetItemLengths = new int[0];
            var itemInfos = surplusCalculator.Calculate(6, targetItemLengths);

            var expected = new ItemInfo[0];
            CollectionAssert.AreEquivalent(expected, itemInfos);
        }

        [Test]
        public void CanCalculate3()
        {
            var surplusCalculator = new Calculator();
            var targetItemLengths = new []
            {
                6,
                5,
                3,
                3,
            };
            var itemInfos = surplusCalculator.Calculate(6, targetItemLengths);

            var expected = new []
            {
                new ItemInfo(6, new [] { 6 }),
                new ItemInfo(6, new [] { 5 }),
                new ItemInfo(6, new [] { 3, 3 }),
            };
            CollectionAssert.AreEquivalent(expected, itemInfos);
        }

        [Test]
        public void CanCalculate4()
        {
            var surplusCalculator = new Calculator();
            var targetItemLengths = new []
            {
                7,
                5,
                3,
            };
            Assert.That(() =>
            {
                surplusCalculator.Calculate(6, targetItemLengths);
            }, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void CanCalculate5()
        {
            var surplusCalculator = new Calculator();
            var targetItemLengths = new []
            {
                /*5,
                1,*/
                5,
                1,
                3,
                2,
                1,
                /*4.5,
                1.5,*/
                /*4,
                2,*/
            };
            var itemInfos = surplusCalculator.Calculate(6, targetItemLengths);

            Assert.That(() => itemInfos.Count, Is.EqualTo(2));
            Assert.That(() => ItemInfoHelper.GetSurplus(itemInfos), Is.EqualTo(0));
        }
    }
}
