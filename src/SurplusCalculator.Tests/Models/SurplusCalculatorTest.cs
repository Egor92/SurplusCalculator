﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            var targetItemCountsByLengths = new Dictionary<int, int>
            {
                [6] = 1,
                [5] = 1,
                [3] = 1,
            };
            var itemInfos = surplusCalculator.Calculate(6, targetItemCountsByLengths);

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
            var targetItemCountsByLengths = new Dictionary<int, int>();
            var itemInfos = surplusCalculator.Calculate(6, targetItemCountsByLengths);

            var expected = new ItemInfo[0];
            CollectionAssert.AreEquivalent(expected, itemInfos);
        }

        [Test]
        public void CanCalculate3()
        {
            var surplusCalculator = new Calculator();
            var targetItemCountsByLengths = new Dictionary<int, int>
            {
                [6] = 1,
                [5] = 1,
                [3] = 2,
            };
            var itemInfos = surplusCalculator.Calculate(6, targetItemCountsByLengths);

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
            var targetItemCountsByLengths = new Dictionary<int, int>
            {
                [7] = 1,
                [5] = 1,
                [3] = 1,
            };
            Assert.That(() =>
            {
                surplusCalculator.Calculate(6, targetItemCountsByLengths);
            }, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void CanCalculate5()
        {
            var surplusCalculator = new Calculator();
            var targetItemCountsByLengths = new Dictionary<int, int>
            {
                [5] = 1,
                [4] = 1,
                [3] = 1,
                [2] = 2,
                [1] = 2,
            };
            var itemInfos = surplusCalculator.Calculate(6, targetItemCountsByLengths);

            Assert.That(() => itemInfos.Count, Is.EqualTo(3));
            Assert.That(() => itemInfos.Select(ItemInfoHelper.GetFreeLength).Sum(), Is.EqualTo(0));
        }
    }
}
