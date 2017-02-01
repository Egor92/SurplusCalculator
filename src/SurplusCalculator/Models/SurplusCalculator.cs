using System;
using System.Collections.Generic;
using System.Linq;

namespace SurplusCalculator.Models
{
    public class SurplusCalculator
    {
        public IList<ItemInfo> Calculate(double sourceItemLength, IList<double> targetItemLengths)
        {
            targetItemLengths = targetItemLengths.OrderByDescending(x => x)
                                                 .ToList();
            foreach  (var targetItemLength in targetItemLengths)
            {
                if (targetItemLength > sourceItemLength)
                    throw new ArgumentException();
            }

            IList<ItemInfo> minimumItemInfos = GetMinimum(sourceItemLength, targetItemLengths, new List<ItemInfo>());
            return minimumItemInfos;
        }

        private IList<ItemInfo> GetMinimum(double sourceItemLength, IList<double> targetItemLengths, IList<ItemInfo> actualItemInfos)
        {
            if (targetItemLengths.Count == 0)
                return actualItemInfos;

            IList<ItemInfo> actualMinimumItemInfos = null;

            for (int i = 0; i < targetItemLengths.Count; i++)
            {
                var currentItemLenght = targetItemLengths[i];
                var copiedItemLengths = targetItemLengths.ToList();
                copiedItemLengths.Remove(currentItemLenght);

                foreach (var currentItemInfo in actualItemInfos)
                {
                    var hasFreeLength = currentItemInfo.GetSurplus() >= currentItemLenght;
                    if (hasFreeLength)
                    {
                        var dictionary = actualItemInfos.ToDictionary(x => x, x => new ItemInfo(x));
                        var copiedItemInfos = new List<ItemInfo>(dictionary.Values);
                        dictionary[currentItemInfo].TargetItemLenghts.Add(currentItemLenght);

                        var itemInfos = GetMinimum(sourceItemLength, copiedItemLengths, copiedItemInfos);
                        actualMinimumItemInfos = GetMinimum(actualMinimumItemInfos, itemInfos);
                    }
                }

                {
                    var dictionary = actualItemInfos.ToDictionary(x => x, x => new ItemInfo(x));
                    var copiedItemInfos = new List<ItemInfo>(dictionary.Values)
                    {
                        new ItemInfo(sourceItemLength, currentItemLenght)
                    };

                    var itemInfos = GetMinimum(sourceItemLength, copiedItemLengths, copiedItemInfos);
                    actualMinimumItemInfos = GetMinimum(actualMinimumItemInfos, itemInfos);
                }
            }

            return actualMinimumItemInfos;
        }

        private IList<ItemInfo> GetMinimum(IList<ItemInfo> left, IList<ItemInfo> right)
        {
            var leftSurplus = ItemInfoHelper.GetSurplus(left);
            var rightSurplus = ItemInfoHelper.GetSurplus(right);
            return leftSurplus < rightSurplus
                ? left
                : right;
        }
    }
}
