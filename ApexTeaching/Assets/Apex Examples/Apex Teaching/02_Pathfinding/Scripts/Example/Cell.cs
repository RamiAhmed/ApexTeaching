namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Cell
    {
        public Vector3 position;
        public int xIndex;
        public int zIndex;
        public bool blocked;

        private readonly List<Cell> _neighbours = new List<Cell>(4);
        private readonly Bounds _bounds;
        private readonly Vector3 _size;

        public Vector3 size
        {
            get { return _size; }
        }

        public List<Cell> neighbours
        {
            get { return _neighbours; }
        }

        public Bounds bounds
        {
            get { return _bounds; }
        }

        public Cell(Vector3 position, int cellSize, int xIndex, int zIndex)
        {
            this.position = position;
            this.blocked = false;
            this.xIndex = xIndex;
            this.zIndex = zIndex;

            _size = Vector3.one * cellSize;
            _bounds = new Bounds(position, _size);
            _size.y = 0.1f;
        }

        public void AddNeighbour(Cell neighbour)
        {
            if (neighbour.blocked)
            {
                return;
            }

            if (!_neighbours.Contains(neighbour))
            {
                _neighbours.Add(neighbour);
            }
        }
    }
}