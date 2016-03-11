namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class SteerForPath : MonoBehaviour
    {
        public float angularSpeed = 5f;
        public float speed = 6f;
        public float arrivalDistance = 1f;

        private Vector3 _currentDestination;

        public Path path
        {
            get;
            private set;
        }

        private void FixedUpdate()
        {
            if (this.path == null)
            {
                return;
            }

            if (this.path.Count == 0)
            {
                this.path = null;
                return;
            }

            var currentDirection = (_currentDestination - this.transform.position);
            var currentDistance = currentDirection.magnitude;
            if (currentDistance < this.arrivalDistance)
            {
                _currentDestination = this.path.Pop();
                return;
            }

            var velocity = (currentDirection / currentDistance) * Mathf.Min(this.speed, currentDistance);
            this.transform.position += velocity * Time.fixedDeltaTime;

            var rotation = Quaternion.LookRotation(velocity, Vector3.up);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.fixedDeltaTime * this.angularSpeed);
        }

        public void SetDestination(Vector3 destination)
        {
            var grid = Grid.instance;
            var currentCell = grid.GetCell(this.transform.position);
            if (currentCell == null || currentCell.blocked)
            {
                currentCell = grid.GetNearestWalkableCell(this.transform.position);
            }

            this.path = grid.FindPath(currentCell.position, destination);
            if (this.path != null && this.path.Count > 0)
            {
                _currentDestination = this.path.Pop();
            }
        }
    }
}