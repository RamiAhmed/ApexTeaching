namespace Apex.AI.Teaching
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Path
    {
        private List<PathCell> _path = new List<PathCell>(10);

        private List<PathCell> _visited;
        private List<PathCell> _openCells;

        private PathCell _destinationCell;
        private PathCell _currentCell;



        public List<PathCell> list
        {
            get { return _path; }
        }

        public Path(PathGrid grid, Vector3 start, Vector3 destination)
        {
            var startCell = grid.GetCell(start);
            if (startCell == null)
            {
                throw new ArgumentNullException("start", this.ToString() + " could not find a cell at the start given: " + start);
            }

            var destinationCell = grid.GetCell(destination);
            if (destinationCell == null)
            {
                throw new ArgumentNullException("destination", this.ToString() + " could not find a cell at the destination given: " + destination);
            }

            _visited = new List<PathCell>(grid.cellCount);
            _openCells = new List<PathCell>(grid.cellCount);

            _visited.Add(startCell);
            _destinationCell = destinationCell;

            FindPath();
        }

        private void FindPath()
        {
            while (Application.isPlaying)
            {
                var neighbours = _currentCell.neighbours;
                var count = neighbours.Count;
                for (int i = 0; i < count; i++)
                {
                    var neighbour = neighbours[i];
                    if (ReferenceEquals(neighbour, _destinationCell))
                    {
                        // Path found! 
                        return;
                    }

                    if (_visited.Contains(neighbour))
                    {
                        continue;
                    }

                    var distance = (_currentCell.position - neighbour.position).magnitude;
                }
            }
        }
    }
}