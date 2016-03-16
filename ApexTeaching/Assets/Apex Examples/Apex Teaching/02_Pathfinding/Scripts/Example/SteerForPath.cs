namespace Apex.AI.Teaching
{
    using UnityEngine;

    [RequireComponent(typeof(UnitBase))]
    [RequireComponent(typeof(SteerableUnit))]
    public sealed class SteerForPath : MonoBehaviour, ISteeringComponent
    {
        public float angularSpeed = 5f;
        public float speed = 6f;
        public float arrivalDistance = 1f;

        [SerializeField]
        private int _priority = 1;

        private Vector3 _currentDestination;

        /// <summary>
        /// Gets the current path if valid, null otherwise.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public Path path
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int priority
        {
            get { return _priority; }
        }

        // Just for being eable to disable component in Unity inspector
        private void OnEnable()
        {
        }

        /// <summary>
        /// Sets the current destination for the unit with this SteerForPath component, thus this will trigger a path request and make the unit start moving along the path.
        /// </summary>
        /// <param name="destination">The destination.</param>
        public void SetDestination(Vector3 destination)
        {
            var grid = Grid.instance;
            var currentCell = grid.GetCell(this.transform.position);
            if (currentCell == null || currentCell.blocked)
            {
                // The starting cell (where the unit currently is) is invalid or blocked, so get the nearest valid one
                currentCell = grid.GetNearestWalkableCell(this.transform.position);
            }

            // Find a path, if possible, to the given destination
            this.path = grid.FindPath(currentCell.position, destination);
            if (this.path != null && this.path.Count > 0)
            {
                // if the path is valid, let's start moving to the first path node
                _currentDestination = this.path.Pop();
            }
        }

        public Vector3? GetSteering(SteeringInput input)
        {
            if (!this.enabled || !this.gameObject.activeSelf)
            {
                // If this component or this game object is disabled, don't do steering
                return null;
            }

            if (this.path == null)
            {
                // no valid path, nothing to do
                return null;
            }

            if (this.path.Count == 0)
            {
                // no path nodes in the path, so null it
                this.path = null;
                return null;
            }

            // Get the distance to the next path node, if the distance is lower than the arrival distance, we can move on to the next point by popping on the path
            var currentDirection = (_currentDestination - this.transform.position);
            var currentDistance = currentDirection.magnitude;
            if (currentDistance < this.arrivalDistance)
            {
                _currentDestination = this.path.Pop();
                return null;
            }

            // Velocity is a vector in the direction from the current location to the next destination, with a length of speed capped to the current distance if we are at the last path node - for slowdown
            var speed = this.path.Count == 1 ? Mathf.Clamp(this.speed, currentDistance, 1f) : this.speed;
            var velocity = (currentDirection / currentDistance) * speed;
            return velocity;
        }
    }
}