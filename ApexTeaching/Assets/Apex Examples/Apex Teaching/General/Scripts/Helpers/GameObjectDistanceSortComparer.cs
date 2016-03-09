namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public struct GameObjectDistanceSortComparer : IComparer<GameObject>
    {
        private Vector3 _position;

        public GameObjectDistanceSortComparer(Vector3 position)
        {
            _position = position;
        }

        public int Compare(GameObject x, GameObject y)
        {
            return (y.transform.position - _position).sqrMagnitude.CompareTo((x.transform.position - _position).sqrMagnitude);
        }
    }
}