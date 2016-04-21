namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Grid : MonoBehaviour
    {
        public static Grid instance;

        private readonly Pathfinder _pathfinder = new Pathfinder();

        public int cellSize = 2;
        public Vector2 gridSize = Vector2.one * 100f;
        public LayerMask obstaclesLayer;

        protected Cell[,] _cells;

        /// <summary>
        /// Gets all cells in a multi-dimensional array.
        /// </summary>
        /// <value>
        /// The cells.
        /// </value>
        public Cell[,] cells
        {
            get { return _cells; }
        }

        protected virtual void OnEnable()
        {
            // The grid is a singleton - there will only ever be one of it
            if (instance != null)
            {
                Debug.LogWarning(this.ToString() + " another Grid has already registered, destroying the old one");
                Destroy(instance, 0.01f);
            }

            instance = this;

            // Calculate the desired size of the grid in 'steps' rather than units/meters
            var xSteps = Mathf.FloorToInt(gridSize.x / cellSize);
            var zSteps = Mathf.FloorToInt(gridSize.y / cellSize);

            // Calculate the starting X and Z coordinates for the grid (half of its size in order to center it)
            var startX = Mathf.CeilToInt(gridSize.x * -0.5f);
            var startZ = Mathf.CeilToInt(gridSize.y * -0.5f);

            // Allocate the necessary memory for the grid
            _cells = new Cell[xSteps, zSteps];

            // Populate the cells array with cells at the correct positions
            for (int x = 0; x < xSteps; x++)
            {
                for (int z = 0; z < zSteps; z++)
                {
                    var xPos = startX + (x * this.cellSize);
                    var zPos = startZ + (z * this.cellSize);
                    _cells[x, z] = new Cell(new Vector3(xPos, 0f, zPos), this.cellSize, x, z);
                }
            }

            // Find all colliders in the scene - they might be obstacles that should block cells
            var colliders = FindObjectsOfType<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                var layer = 1 << collider.gameObject.layer;
                if ((obstaclesLayer & layer) == 0)
                {
                    // if the collider is not in the 'obstaclesLayer' it is not an obstacle that cells should consider
                    colliders[i] = null;
                    continue;
                }

                var cell = GetCell(collider.transform.position);
                if (cell != null && !cell.blocked)
                {
                    // If the collider's center position is within the cell's bounds object, the cell is definitely blocked
                    cell.blocked = true;
                }
            }

            // Iterate through all cells to check whether their bounds overlap with any of the identified obstacle collider's bounds
            for (int x = 0; x < xSteps; x++)
            {
                for (int z = 0; z < zSteps; z++)
                {
                    var cell = _cells[x, z];
                    if (cell.blocked)
                    {
                        continue;
                    }

                    // colliders' loop
                    var cellBounds = cell.bounds;
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        var coll = colliders[i];
                        if (coll == null)
                        {
                            continue;
                        }

                        if (cellBounds.Intersects(coll.bounds))
                        {
                            // the cell's bounds intersects with the collider's bounds, thus the cell should be blocked
                            cell.blocked = true;
                            break;
                        }
                    }
                }
            }

            // After identifying blocked cells, each cell must know which walkable cell neighbours it can connect to
            IdentifyCellNeighbours();
        }

        private void IdentifyCellNeighbours()
        {
            var xLength = _cells.GetLength(0);
            var zLength = _cells.GetLength(1);
            for (int x = 0; x < xLength; x++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    var cell = _cells[x, z];
                    var xi = cell.xIndex;
                    var zi = cell.zIndex;

                    // For each cell in the multi-dimensional array, add the cells in each non-diagonal direction (up, down, left and right) as neighbour
                    if (xi > 0)
                    {
                        cell.AddNeighbour(_cells[xi - 1, zi]);
                    }

                    if (xi < xLength - 1)
                    {
                        cell.AddNeighbour(_cells[xi + 1, zi]);
                    }

                    if (zi > 0)
                    {
                        cell.AddNeighbour(_cells[xi, zi - 1]);
                    }

                    if (zi < zLength - 1)
                    {
                        cell.AddNeighbour(_cells[xi, zi + 1]);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the cells' blocked status to unblocked for all cells intersecting with the bounds of the given collider.
        /// </summary>
        /// <param name="collider">The collider.</param>
        public void UpdateCellsBlockedStatus(Collider collider)
        {
            var xSteps = _cells.GetLength(0);
            var zSteps = _cells.GetLength(1);
            for (int x = 0; x < xSteps; x++)
            {
                for (int z = 0; z < zSteps; z++)
                {
                    var cell = _cells[x, z];
                    if (!cell.blocked)
                    {
                        continue;
                    }

                    if (cell.bounds.Intersects(collider.bounds))
                    {
                        cell.blocked = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets all the cells within radius from position. The passed list is populated with the cells.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="list">The list to populate.</param>
        public virtual void GetUnblockedCells(Vector3 position, float radius, IList<Cell> list)
        {
            list.Clear();
            var radiusSqr = radius * radius;

            var xLength = _cells.GetLength(0);
            var zLength = _cells.GetLength(1);
            for (int x = 0; x < xLength; x++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    var cell = _cells[x, z];
                    if (cell.blocked)
                    {
                        continue;
                    }

                    if ((cell.position - position).sqrMagnitude < radiusSqr)
                    {
                        list.Add(cell);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a cell at the given coordinates by checking whether the given position is within the cell's bounds object.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>A cell if one is found at the position, null otherwise.</returns>
        public virtual Cell GetCell(Vector3 position)
        {
            var xLength = _cells.GetLength(0);
            var zLength = _cells.GetLength(1);
            for (int x = 0; x < xLength; x++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    var cell = _cells[x, z];
                    if (cell.bounds.Contains(new Vector3(position.x, cell.position.y, position.z)))
                    {
                        return cell;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the nearest walkable/unblocked cell to the given position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The nearest unblocked cell to the given position</returns>
        public virtual Cell GetNearestWalkableCell(Vector3 position)
        {
            var shortest = float.MaxValue;
            Cell closest = null;

            var xLength = _cells.GetLength(0);
            var zLength = _cells.GetLength(1);
            for (int x = 0; x < xLength; x++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    var cell = _cells[x, z];
                    if (cell.blocked)
                    {
                        continue;
                    }

                    var distance = (cell.position - position).sqrMagnitude;
                    if (distance < shortest)
                    {
                        shortest = distance;
                        closest = cell;
                    }
                }
            }

            return closest;
        }

        /// <summary>
        /// Finds a path from start to destination.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>A path if succesful, null otherwise.</returns>
        public virtual Path FindPath(Vector3 start, Vector3 destination)
        {
            return _pathfinder.FindPath(this, start, destination);
        }
    }
}