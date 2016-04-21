namespace Apex.AI.Teaching
{
    using UnityEngine;

    [RequireComponent(typeof(UnitBase))]
    [RequireComponent(typeof(SteerableUnit))]
    public sealed class SteerForBlockAvoidance : MonoBehaviour, ISteeringComponent
    {
        [SerializeField]
        private int _priority = 15;

        private UnitBase _unit;

        /// <summary>
        /// Gets the priority of this particular steering component. The priority controls whether this steering component is executed. Higher priority steering components get executed first, and the first one with a value is used.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int priority
        {
            get { return _priority; }
        }

        private void OnEnable()
        {
            // Get a reference to the unit
            _unit = this.GetComponent<UnitBase>();
        }

        public Vector3? GetSteering(SteeringInput input)
        {
            if (!this.enabled || !this.gameObject.activeSelf)
            {
                // If this component or this game object is disabled, don't do steering
                return null;
            }

            var grid = Grid.instance;
            var position = _unit.transform.position;
            var currentCell = grid.GetCell(position);
            if (currentCell == null || currentCell.blocked)
            {
                // cannot resolve when already on an invalid/blocked cell
                return null;
            }

            // Iterate through all neighbour cells compared to our current cell
            var avoidVector = Vector3.zero;
            var xLength = grid.cells.GetLength(0);
            var zLength = grid.cells.GetLength(1);
            var xIdx = currentCell.xIndex;
            var zIdx = currentCell.zIndex;
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0)
                    {
                        // ignore the 'cell at the center', since it is the same as the current cell
                        continue;
                    }

                    var ix = xIdx + x;
                    var iz = zIdx + z;

                    if (ix < 0 || iz < 0 || ix >= xLength || iz >= zLength)
                    {
                        // skip cells that are technically outside of the grid
                        continue;
                    }

                    var cell = grid.cells[ix, iz];
                    if (cell.blocked)
                    {
                        // sum up all vectors from blocked cells
                        avoidVector += (position - cell.position);
                    }
                }
            }

            if (avoidVector.sqrMagnitude == 0f)
            {
                // no avoid vectors accumulated
                return null;
            }
            
            return avoidVector.normalized * input.speed;
        }
    }
}