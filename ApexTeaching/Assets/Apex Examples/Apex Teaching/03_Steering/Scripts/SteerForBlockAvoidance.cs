namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class SteerForBlockAvoidance : MonoBehaviour, ISteeringComponent
    {
        [SerializeField]
        private int _priority = 15;

        private UnitBase _unit;

        public int priority
        {
            get { return _priority; }
        }

        private void OnEnable()
        {
            _unit = this.GetComponent<UnitBase>();
        }

        public Vector3? GetSteering(SteeringInput input)
        {
            if (!this.enabled || !this.gameObject.activeSelf)
            {
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
                        continue;
                    }

                    var ix = xIdx + x;
                    var iz = zIdx + z;

                    if (ix < 0 || iz < 0 || ix >= xLength || iz >= zLength)
                    {
                        continue;
                    }

                    var cell = grid.cells[ix, iz];
                    if (cell.blocked)
                    {
                        avoidVector += (position - cell.position);
                    }
                }
            }

            if (avoidVector.sqrMagnitude == 0f)
            {
                return null;
            }

            return avoidVector.normalized * input.speed;
        }
    }
}