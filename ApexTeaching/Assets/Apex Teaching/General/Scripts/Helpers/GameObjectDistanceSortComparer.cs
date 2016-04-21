namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public struct GameObjectDistanceSortComparer : IComparer<GameObject>
    {
        private Vector3 _position;
        private int _sortDir;

        public GameObjectDistanceSortComparer(Vector3 position, bool ascending = true)
        {
            _position = position;
            _sortDir = ascending ? 1 : -1;
        }

        public int Compare(GameObject a, GameObject b)
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