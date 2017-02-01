using System;
using NUnit.Framework;
using SurplusCalculator.Models;
using Calculator = SurplusCalculator.Models.SurplusCalculator;

namespace SurplusCalculator.Test.Models
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
            var targetItemLengths = new double[]
            {
                6,
                5,
                3,
            };
            var itemInfos = surplusCalculator.Calculate(6, targetItemLengths);

            var expected = new[]
            {
                new ItemInfo(6, new double[] { 6 }),
                new ItemInfo(6, new double[] { 5 }),
                new ItemInfo(6, new double[] { 3 }),
            };
            CollectionAssert.AreEquivalent(expected, itemInfos);
        }

        [Test]
        public void CanCalculate2()
        {
            var surplusCalculator = new Calculator();
            var targetItemLengths = new double[0];
            var itemInfos = surplusCalculator.Calculate(6, targetItemLengths);

            var expected = new ItemInfo[0];
            CollectionAssert.AreEquivalent(expected, itemInfos);
        }

        [Test]
        public void CanCalculate3()
        {
            var surplusCalculator = new Calculator();
            var targetItemLengths = new double[]
            {
                6,
                5,
                3,
                3,
            };
            var itemInfos = surplusCalculator.Calculate(6, targetItemLengths);

            var expected = new []
            {
                new ItemInfo(6, new double[] { 6 }),
                new ItemInfo(6, new double[] { 5 }),
                new ItemInfo(6, new double[] { 3, 3 }),
            };
            CollectionAssert.AreEquivalent(expected, itemInfos);
        }

        [Test]
        public void CanCalculate4()
        {
            var surplusCalculator = new Calculator();
            var targetItemLengths = new double[]
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
            var targetItemLengths = new double[]
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
                3.72,
                2.28,
            };
            var itemInfos = surplusCalculator.Calculate(6, targetItemLengths);

            Assert.That(() => itemInfos.Count, Is.EqualTo(3));
            Assert.That(() => ItemInfoHelper.GetSurplus(itemInfos), Is.EqualTo(0));
        }
    }
}
