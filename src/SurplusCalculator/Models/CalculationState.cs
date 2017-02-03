using System.Collections.Generic;

namespace SurplusCalculator.Models
{
    public class CalculationState
    {
        public IList<ItemInfo> MinimumItemInfos { get; set; }

        public HashSet<string> VisitedStateHashes { get; set; } = new HashSet<string>();

        public int ItemCount
        {
            get { return MinimumItemInfos?.Count ?? int.MaxValue; }
        }
    }
}
