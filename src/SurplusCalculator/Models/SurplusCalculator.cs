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

            CalculationState calculationState = new CalculationState();
            FindMinimum(sourceItemLength, targetItemCountsByLengths, new List<ItemInfo>(), calculationState);
            return calculationState.MinimumItemInfos;
        }

        private void FindMinimum(int sourceItemLength, 
                                 IDictionary<int, int> targetItemCountsByLengths, 
                                 IList<ItemInfo> actualItemInfos, 
                                 CalculationState calculationState)
        {
            if (targetItemCountsByLengths.Count == 0)
            {
                var isActualItemInfosMinimum = actualItemInfos.Count < calculationState.ItemCount;
                if (isActualItemInfosMinimum)
                {
                    calculationState.MinimumItemInfos = actualItemInfos;
                }
                return;
            }

            var stateHash = ItemInfoHelper.GetHash(actualItemInfos);
            var isStateVisited = calculationState.VisitedStateHashes.Contains(stateHash);
            if (isStateVisited)
                return;
            calculationState.VisitedStateHashes.Add(stateHash);

            foreach (var pair in targetItemCountsByLengths)
            {
                var currentItemLength = pair.Key;

                var copiedItemCountsByLengths = new Dictionary<int, int>(targetItemCountsByLengths);
                copiedItemCountsByLengths[currentItemLength]--;
                if (copiedItemCountsByLengths[currentItemLength] == 0)
                {
                    copiedItemCountsByLengths.Remove(currentItemLength);
                }

                foreach (var currentItemInfo in actualItemInfos)
                {
                    var hasFreeLength = ItemInfoHelper.GetFreeLength(currentItemInfo) >= currentItemLength;
                    if (!hasFreeLength)
                        continue;

                    var dictionary = actualItemInfos.ToDictionary(x => x, x => new ItemInfo(x));
                    var copiedItemInfos = new List<ItemInfo>(dictionary.Values);
                    dictionary[currentItemInfo].TargetItemLenghts.Add(currentItemLength);

                    FindMinimum(sourceItemLength, copiedItemCountsByLengths, copiedItemInfos, calculationState);
                }
                {
                    var dictionary = actualItemInfos.ToDictionary(x => x, x => new ItemInfo(x));
                    var copiedItemInfos = new List<ItemInfo>(dictionary.Values)
                    {
                        new ItemInfo(sourceItemLength, currentItemLength)
                    };

                    FindMinimum(sourceItemLength, copiedItemCountsByLengths, copiedItemInfos, calculationState);
                }
            }
        }
    }
}
