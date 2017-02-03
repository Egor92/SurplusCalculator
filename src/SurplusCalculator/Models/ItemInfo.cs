using System;
using System.Collections.Generic;
using System.Linq;

namespace SurplusCalculator.Models
{
    public class ItemInfo : IEquatable<ItemInfo>
    {
        #region Ctor

        public ItemInfo(int sourceItemLenght, int targetItemLenghts)
        {
            SourceItemLenght = sourceItemLenght;
            TargetItemLenghts = new List<int>
            {
                targetItemLenghts
            };
        }

        public ItemInfo(ItemInfo other)
        {
            SourceItemLenght = other.SourceItemLenght;
            TargetItemLenghts = new List<int>(other.TargetItemLenghts);
        }

        public ItemInfo(int sourceItemLenght, IEnumerable<int> targetItemLenghts)
        {
            SourceItemLenght = sourceItemLenght;
            TargetItemLenghts = new List<int>(targetItemLenghts);
        }

        #endregion

        public int SourceItemLenght { get; }

        public List<int> TargetItemLenghts { get; }

        #region Overrides of Object

        public override string ToString()
        {
            return $"{{{string.Join(", ", TargetItemLenghts)}}}";
        }

        #endregion

        public double GetSurplus()
        {
            return SourceItemLenght - TargetItemLenghts.Sum();
        }

        #region Implementation of IEquatable<ItemInfo>

        public bool Equals(ItemInfo other)
        {
            if (other == null)
                return false;
            if (SourceItemLenght != other.SourceItemLenght)
                return false;
            if (!TargetItemLenghts.SequenceEqual(other.TargetItemLenghts))
                return false;
            return true;
        }

        #endregion
    }
}
