namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public class UnitBaseDistanceSortComparer : IComparer<UnitBase>
    {
        private Vector3 _position;
        private int _sortDir;

        public UnitBaseDistanceSortComparer(Vector3 position, bool ascending = true)
        {
            _position = position;
            _sortDir = ascending ? 1 : -1;
        }

        public int Compare(UnitBase a, UnitBase b)
        {
            if (a == null)
            {
                return 1 * _sortDir;
            }

            if (b == null)
            {
                return -1 * _sortDir;
            }

            return (a.transform.position - _position).sqrMagnitude
                .CompareTo((b.transform.position - _position).sqrMagnitude) * _sortDir;
        }
    }
}