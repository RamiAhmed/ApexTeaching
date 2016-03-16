namespace Apex.AI.Teaching
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(UnitBase))]
    public sealed class PatrolComponent : MonoBehaviour
    {
        public Transform[] patrolPoints;

        private UnitBase _unit;
        private int _currentIdx;

        private void Awake()
        {
            if (this.patrolPoints == null || this.patrolPoints.Length == 0)
            {
                throw new ArgumentNullException("patrolPoints");
            }

            _unit = this.GetComponent<UnitBase>();
            if (_unit == null)
            {
                throw new ArgumentNullException("_unit");
            }
        }

        private void Update()
        {
            if (_unit.isMoving)
            {
                // unit already on the move
                return;
            }

            // move to next patrol point
            if (_currentIdx >= patrolPoints.Length)
            {
                _currentIdx = 0;
            }

            // Make the unit move to the next point in the patrolPoints array, and increment the indexer afterwards (post-incrementation)
            var point = this.patrolPoints[_currentIdx++];
            _unit.MoveTo(point.position);
        }
    }
}