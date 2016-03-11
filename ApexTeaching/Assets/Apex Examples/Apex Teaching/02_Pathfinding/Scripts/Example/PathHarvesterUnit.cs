namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class PathHarvesterUnit : HarvesterUnit
    {
        private SteerForPath _steerForPath;

        private void Start()
        {
            _steerForPath = this.GetComponent<SteerForPath>();
        }

        public override bool isMoving
        {
            get { return _steerForPath.path != null; }
        }

        public override void MoveTo(Vector3 destination)
        {
            var cell = PathGrid.instance.GetCell(destination);
            if (cell == null || cell.blocked)
            {
                cell = PathGrid.instance.GetNearestWalkableCell(destination);
            }

            _steerForPath.SetDestination(cell.position);
        }

        public override void RandomWander()
        {
            var randomPos = this.transform.position + Random.onUnitSphere.normalized * _randomWanderRadius;
            randomPos.y = this.transform.position.y;
            MoveTo(randomPos);
        }

        public override void StopMoving()
        {
            _steerForPath.path.Clear();
        }
    }
}