namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PathCell
    {
        public Vector3 position;
        public int xIndex;
        public int zIndex;
        public bool blocked;

        private List<PathCell> _neighbours = new List<PathCell>(4);
        private Bounds _bounds;
        private Vector3 _size;

        public Vector3 size
        {
            get { return _size; }
        }

        public List<PathCell> neighbours
        {
            get { return _neighbours; }
        }

        public Bounds bounds
        {
            get { return _bounds; }
        }

        public PathCell(Vector3 position, int cellSize, int xIndex, int zIndex)
        {
            this.position = position;
            this.blocked = false;
            this.xIndex = xIndex;
            this.zIndex = zIndex;

            _size = Vector3.one * cellSize;
            _bounds = new Bounds(position, _size);
            _size.y = 0.1f;
        }

        public void AddNeighbour(PathCell neighbour)
        {
            if (!_neighbours.Contains(neighbour))
            {
                _neighbours.Add(neighbour);
            }
        }

        public bool Contains(Vector3 position)
        {
            return _bounds.Contains(position);
        }
    }
}