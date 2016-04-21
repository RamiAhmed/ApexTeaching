namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class Cell
    {
        public Vector3 position;
        public int xIndex;
        public int zIndex;
        public bool blocked;

        private readonly List<Cell> _neighbours = new List<Cell>(4);
        private readonly Bounds _bounds;
        private readonly Vector3 _size;

        /// <summary>
        /// Gets the size of this cell.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public Vector3 size
        {
            get { return _size; }
        }

        /// <summary>
        /// Gets a list of all the cell neighbours.
        /// </summary>
        /// <value>
        /// The neighbours.
        /// </value>
        public List<Cell> neighbours
        {
            get { return _neighbours; }
        }

        /// <summary>
        /// Gets the bounds object for this cell.
        /// </summary>
        /// <value>
        /// The bounds.
        /// </value>
        public Bounds bounds
        {
            get { return _bounds; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="cellSize">Size of the cell.</param>
        /// <param name="xIndex">X index in the multi-dimensional array.</param>
        /// <param name="zIndex">Z index in the multi-dimensional array.</param>
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

        /// <summary>
        /// Adds the given neighbour, unless it is blocked or already registered as a neighbour.
        /// </summary>
        /// <param name="neighbour">The neighbour.</param>
        public void AddNeighbour(Cell neighbour)
        {
            if (!neighbour.blocked && !_neighbours.Contains(neighbour))
            {
                _neighbours.Add(neighbour);
            }
        }
    }
}