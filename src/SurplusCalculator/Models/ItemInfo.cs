using System;
using System.Collections.Generic;
using System.Linq;

namespace SurplusCalculator.Models
{
    public class ItemInfo : IEquatable<ItemInfo>
    {
        #region Ctor

        public ItemInfo(double sourceItemLenght, double targetItemLenghts)
        {
            SourceItemLenght = sourceItemLenght;
            TargetItemLenghts = new List<double>
            {
                targetItemLenghts
            };
        }

        public ItemInfo(ItemInfo other)
        {
            SourceItemLenght = other.SourceItemLenght;
            TargetItemLenghts = new List<double>(other.TargetItemLenghts);
        }

        public ItemInfo(double sourceItemLenght, IEnumerable<double> targetItemLenghts)
        {
            SourceItemLenght = sourceItemLenght;
            TargetItemLenghts = new List<double>(targetItemLenghts);
        }

        #endregion

        public double SourceItemLenght { get; }

        public List<double> TargetItemLenghts { get; }

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
            if (SourceItemLenght != SourceItemLenght)
                return false;
            if (!TargetItemLenghts.SequenceEqual(other.TargetItemLenghts))
                return false;
            return true;
        }

        #endregion
    }
}
