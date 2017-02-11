using System.Collections.Generic;

namespace SurplusCalculator.Models
{
    public interface ISurplusCalculator
    {
        IList<ItemInfo> Calculate(int sourceItemLength, IDictionary<int, int> targetItemCountsByLengths);
    }
}