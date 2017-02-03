using System;
using System.Collections.Generic;
using System.Linq;

namespace SurplusCalculator.Models
{
    public class SurplusCalculator
    {
        public IList<ItemInfo> Calculate(int sourceItemLength, IDictionary<int, int> targetItemCountsByLengths)
        {
            foreach (var pair in targetItemCountsByLengths)
            {
                var targetItemLength = pair.Key;
                if (targetItemLength > sourceItemLength)
                    throw new ArgumentException();
            }

            IList<ItemInfo> minimumItemInfos = GetMinimum(sourceItemLength, targetItemCountsByLengths, new List<ItemInfo>());
            return minimumItemInfos;
        }

        private IList<ItemInfo> GetMinimum(int sourceItemLength, IDictionary<int, int> targetItemCountsByLengths, IList<ItemInfo> actualItemInfos)
        {
            if (targetItemCountsByLengths.Count == 0)
                return actualItemInfos;

            IList<ItemInfo> actualMinimumItemInfos = null;

            foreach (var pair in targetItemCountsByLengths)
            {
                var currentItemLength = pair.Key;
                var currentItemCount = pair.Value;

                var copiedItemCountsByLengths = new Dictionary<int, int>(targetItemCountsByLengths);
                copiedItemCountsByLengths[currentItemLength]--;
                if (copiedItemCountsByLengths[currentItemLength] == 0)
                    copiedItemCountsByLengths.Remove(currentItemLength);

                foreach (var currentItemInfo in actualItemInfos)
                {
                    var hasFreeLength = currentItemInfo.GetSurplus() >= currentItemLength;
                    if (hasFreeLength)
                    {
                        var dictionary = actualItemInfos.ToDictionary(x => x, x => new ItemInfo(x));
                        var copiedItemInfos = new List<ItemInfo>(dictionary.Values);
                        dictionary[currentItemInfo].TargetItemLenghts.Add(currentItemLength);

                        var itemInfos = GetMinimum(sourceItemLength, copiedItemCountsByLengths, copiedItemInfos);
                        actualMinimumItemInfos = GetMinimum(actualMinimumItemInfos, itemInfos);
                    }
                }
                {
                    var dictionary = actualItemInfos.ToDictionary(x => x, x => new ItemInfo(x));
                    var copiedItemInfos = new List<ItemInfo>(dictionary.Values)
                    {
                        new ItemInfo(sourceItemLength, currentItemLength)
                    };

                    var itemInfos = GetMinimum(sourceItemLength, copiedItemCountsByLengths, copiedItemInfos);
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
