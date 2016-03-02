namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public struct GameObjectDistanceSortComparer : IComparer<GameObject>
    {
        private Vector3 position;

        public GameObjectDistanceSortComparer(Vector3 position)
        {
            this.position = position;
        }

        public int Compare(GameObject x, GameObject y)
        {
            return (x.transform.position - position).sqrMagnitude.CompareTo((y.transform.position - position).sqrMagnitude);
        }
    }
}