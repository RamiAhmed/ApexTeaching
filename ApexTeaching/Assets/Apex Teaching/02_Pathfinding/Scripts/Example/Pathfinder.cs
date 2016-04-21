namespace Apex.AI.Teaching
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Pathfinder
    {
        private const int maxIterations = 1000;

        private readonly List<Cell> _visited = new List<Cell>(40);
        private readonly List<PathNode> _openNodes = new List<PathNode>(40);

        private Cell _destinationCell;
        private PathNode _currentNode;
        private Path _lastPath;

        /// <summary>
        /// Finds the path.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="start">The start.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// start
        /// or
        /// destination
        /// </exception>
        public Path FindPath(Grid grid, Vector3 start, Vector3 destination)
        {
            // Ensure that start and destination cells are valid and not blocked
            var startCell = grid.GetCell(start);
            if (startCell == null || startCell.blocked)
            {
                throw new ArgumentNullException("start", this.ToString() + " could not find an unblocked cell at the start given: " + start);
            }

            var destinationCell = grid.GetCell(destination);
            if (destinationCell == null || destinationCell.blocked)
            {
                throw new ArgumentNullException("destination", this.ToString() + " could not find an unblocked cell at the destination given: " + destination);
            }

            // Store the starting cell as a path node and mark it as visited, and save the destination cell for later
            _destinationCell = destinationCell;
            _currentNode = new PathNode(null, startCell, 0f);
            _visited.Add(_currentNode.cell);

            // keep iterating until a path is found - or Unity is stopped
            var counter = 0;
            while (Application.isPlaying)
            {
                if (FindPathIteration())
                {
                    break;
                }

                if (counter++ > maxIterations)
                {
                    // "fail-safe" to avoid the pathfinder from consuming too many resources (avoid too many iterations), especially for cases where there is no possible path
                    Debug.LogWarning(this.ToString() + " could not find a path. Max tries exceeded");
                    break;
                }
            }

            // Reset everything for next time a path is needed
            Reset();

            // return the identified path
            return _lastPath;
        }

        private bool FindPathIteration()
        {
            // Get all the current node's cell neighbours
            var neighbours = _currentNode.cell.neighbours;
            var count = neighbours.Count;
            if (count == 0)
            {
                // Path not found
                Debug.LogWarning(this.ToString() + " could not find a path. No neighbours to start cell found");
                return true;
            }

            // Iterate through the list of cell neighbours
            for (int i = 0; i < count; i++)
            {
                var neighbour = neighbours[i];
                if (ReferenceEquals(neighbour, _destinationCell))
                {
                    // The cell neighbour matches the destination cell => Path found!
                    BuildPath();
                    return true;
                }

                if (_visited.Contains(neighbour))
                {
                    // the neighbour has already been visited previously
                    continue;
                }

                // calculate the cost for this neighbour and add it as a path node to the open collection
                var cost = GetCost(neighbour, _currentNode.cell);
                var node = new PathNode(_currentNode, neighbour, cost);
                _openNodes.Add(node);
            }

            // Iterate through all open nodes in order to identify the one with the lowest cost
            var openNodesCount = _openNodes.Count;
            var lowestCost = float.MaxValue;
            for (int j = 0; j < openNodesCount; j++)
            {
                var node = _openNodes[j];
                var cost = node.cost;
                if (cost < lowestCost)
                {
                    lowestCost = cost;
                    _currentNode = node;
                }
            }

            // The current node is no longer in the open collection - it is the next one to be evaluated, and thus it has also been visited now
            _openNodes.Remove(_currentNode);
            _visited.Add(_currentNode.cell);

            // Still iterating
            return false;
        }

        private void BuildPath()
        {
            // Build the path by going backwards from the destination cell - each path node knows about its 'parent' so use this for iterating
            var path = new Path(_visited.Count);
            var current = new PathNode(_currentNode, _destinationCell, 0f);
            while (current != null)
            {
                path.Add(current.position);
                current = current.parent;
            }

            // reverse the order of the path to make it start from the intended starting cell
            path.Reverse();
            _lastPath = path;
        }

        private void Reset()
        {
            // Reset everything but the last generated path (since it may need to be returned)
            _currentNode = null;
            _destinationCell = null;
            _visited.Clear();
            _openNodes.Clear();
        }

        private float GetCost(Cell fromCell, Cell toCell)
        {
            // first calculates the distance from the given cell to the other, but adds the 'Euclidean' (straight-line) distance as a simple heuristic
            return (toCell.position - fromCell.position).magnitude + (_destinationCell.position - fromCell.position).magnitude;
        }
    }
}