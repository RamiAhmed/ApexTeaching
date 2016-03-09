namespace Apex.AI.Teaching
{
    using UnityEngine;

    public class PathGrid : MonoBehaviour
    {
        public int cellSize = 2;

        public Vector2 gridSize = Vector2.one * 20f;

        public LayerMask obstaclesLayer;

        private PathCell[,] _cells;

        public PathCell[,] cells
        {
            get { return _cells; }
        }

        public int cellCount
        {
            get { return _cells.Length; }
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            var startX = Mathf.CeilToInt(gridSize.x * -0.5f);
            var xSteps = Mathf.FloorToInt(gridSize.x / cellSize);

            var startZ = Mathf.CeilToInt(gridSize.y * -0.5f);
            var zSteps = Mathf.FloorToInt(gridSize.y / cellSize);

            _cells = new PathCell[xSteps, zSteps];

            for (int x = 0; x < xSteps; x++)
            {
                for (int z = 0; z < zSteps; z++)
                {
                    var xPos = startX + (x * this.cellSize);
                    var zPos = startZ + (z * this.cellSize);
                    _cells[x, z] = new PathCell(new Vector3(xPos, 0f, zPos), this.cellSize, x, z);
                }
            }

            var colliders = FindObjectsOfType<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                if (((1 << obstaclesLayer) & (1 << collider.gameObject.layer)) != 0)
                {
                    continue;
                }

                var cell = GetCell(collider.transform.position);
                if (cell != null && !cell.blocked)
                {
                    cell.blocked = true;
                }
            }

            IdentifyCellNeighbours();
        }

        private void IdentifyCellNeighbours()
        {
            var xSteps = Mathf.FloorToInt(gridSize.x / cellSize) - 1;
            var zSteps = Mathf.FloorToInt(gridSize.y / cellSize) - 1;

            foreach (var cell in _cells)
            {
                var xi = cell.xIndex;
                var zi = cell.zIndex;

                if (xi > 0)
                {
                    cell.AddNeighbour(_cells[xi - 1, zi]);
                }

                if (xi < xSteps)
                {
                    cell.AddNeighbour(_cells[xi + 1, zi]);
                }

                if (zi > 0)
                {
                    cell.AddNeighbour(_cells[xi, zi - 1]);
                }

                if (zi < zSteps)
                {
                    cell.AddNeighbour(_cells[xi, zi + 1]);
                }
            }
        }

        public PathCell GetCell(Vector3 position)
        {
            foreach (var cell in _cells)
            {
                if (cell.Contains(new Vector3(position.x, cell.position.y, position.z)))
                {
                    return cell;
                }
            }

            return null;
        }

        public Path GetPath(Vector3 start, Vector3 destination)
        {
            return new Path(this, start, destination);
        }
    }
}