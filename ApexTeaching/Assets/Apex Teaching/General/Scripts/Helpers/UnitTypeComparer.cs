namespace Apex.AI.Teaching
{
    using System.Collections.Generic;

    public struct UnitTypeComparer : IEqualityComparer<UnitType>
    {
        public bool Equals(UnitType x, UnitType y)
        {
            return x == y;
        }

        public int GetHashCode(UnitType obj)
        {
            return (int)obj;
        }
    }
}