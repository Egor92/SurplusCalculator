using System;
using System.Collections.Generic;
using System.Linq;

namespace SurplusCalculator.Models
{
    public static class ItemInfoHelper
    {
        public static double GetSurplus(IEnumerable<ItemInfo> itemInfos)
        {
            if (itemInfos == null)
                return double.PositiveInfinity;

            var surpluses = itemInfos.Select(x => x.GetSurplus())
                                   .ToList();
            if (surpluses.Any(x => x < 0))
                throw new Exception("Surplus is negative");
            return surpluses.Sum();
        }
    }
}
