using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace SurplusCalculator.Models
{
    public class ApproximateSurplusCalculator : ISurplusCalculator
    {
        public int AllowableError { get; set; }

        public IList<ItemInfo> Calculate(int sourceItemLength, IDictionary<int, int> targetItemCountsByLengths)
        {
            foreach (var pair in targetItemCountsByLengths)
            {
                var targetItemLength = pair.Key;
                if (targetItemLength > sourceItemLength)
                    throw new ArgumentException();
            }

            targetItemCountsByLengths = new Dictionary<int, int>(targetItemCountsByLengths);
            List<ItemInfo> itemInfos = new List<ItemInfo>();
            while (!IsEmpty(targetItemCountsByLengths))
            {
                IList<int> minimumItemLengths = GetMinimumItemInfo(sourceItemLength, targetItemCountsByLengths);
                while (CanAddItemInfo(targetItemCountsByLengths, minimumItemLengths))
                {
                    itemInfos.Add(new ItemInfo(sourceItemLength, minimumItemLengths));

                    foreach (var targetItemLenght in minimumItemLengths)
                    {
                        targetItemCountsByLengths[targetItemLenght]--;
                    }
                }
            }

            return itemInfos;
        }

        private static bool CanAddItemInfo(IDictionary<int, int> targetItemCountsByLengths, IList<int> targetItemLenghts)
        {
            targetItemCountsByLengths = new Dictionary<int, int>(targetItemCountsByLengths);
            foreach (var targetItemLenght in targetItemLenghts)
            {
                targetItemCountsByLengths[targetItemLenght]--;
            }

            return targetItemCountsByLengths.All(x => x.Value >= 0);
        }

        private IList<int> GetMinimumItemInfo(int sourceItemLength, IDictionary<int, int> targetItemCountsByLengths)
        {
            var maxItemLength = targetItemCountsByLengths.Where(x => x.Value > 0)
                                                         .Max(x => x.Key);
            targetItemCountsByLengths = new Dictionary<int, int>(targetItemCountsByLengths);
            foreach (var pair in targetItemCountsByLengths)
            {
                var itemLength = pair.Key;
                var itemCount = pair.Value;

                if (itemLength <= maxItemLength/2)
                    continue;

                if (itemCount <= 0)
                    continue;

                if (sourceItemLength%itemLength != 0)
                    continue;

                var desiredCount = sourceItemLength/itemLength;
                if (desiredCount > itemCount)
                    continue;

                targetItemCountsByLengths[itemLength] -= desiredCount;
                var targetItemLengths = Enumerable.Repeat(itemLength, desiredCount)
                                                  .ToArray();
                return targetItemLengths;
            }

            return GetMinimumItemInfo(sourceItemLength, targetItemCountsByLengths, new List<int>(), new HashSet<string>());
        }

        private IList<int> GetMinimumItemInfo(int sourceItemLength, 
                                              IDictionary<int, int> targetItemCountsByLengths, 
                                              IList<int> actualItemLengths, 
                                              ICollection<string> visitedStateHashes)
        {
            if (IsEmpty(targetItemCountsByLengths))
            {
                return actualItemLengths;
            }

            var stateHash = GetHash(actualItemLengths);
            var isStateVisited = visitedStateHashes.Contains(stateHash);
            if (isStateVisited)
                return null;
            visitedStateHashes.Add(stateHash);

            List<IList<int>> minimumItemInfos = new List<IList<int>>();
            foreach (var pair in targetItemCountsByLengths)
            {
                var itemLength = pair.Key;
                var itemCount = pair.Value;

                if (itemCount <= 0)
                    continue;

                var copiedItemCountsByLengths = new Dictionary<int, int>(targetItemCountsByLengths);
                copiedItemCountsByLengths[itemLength]--;

                var copiedItemLengths = new List<int>(actualItemLengths)
                {
                    itemLength, 
                };

                var totalLength = GetTotalLength(copiedItemLengths);
                if (totalLength + AllowableError > sourceItemLength)
                    continue;

                var minimumItemInfo = GetMinimumItemInfo(sourceItemLength, copiedItemCountsByLengths, copiedItemLengths, 
                                                         visitedStateHashes);
                if (minimumItemInfo != null)
                {
                    minimumItemInfos.Add(minimumItemInfo);
                }
            }

            if (!minimumItemInfos.Any())
                return actualItemLengths;

            return minimumItemInfos.MaxBy(GetTotalLength);
        }

        private static int GetTotalLength(IList<int> itemLengths)
        {
            return itemLengths.Sum();
        }

        private static string GetHash(IEnumerable<int> itemLengths)
        {
            return $"{{{string.Join(";", itemLengths)}}}";
        }

        private static bool IsEmpty(IDictionary<int, int> targetItemCountsByLengths)
        {
            return targetItemCountsByLengths.All(x => x.Value <= 0);
        }
    }
}
