namespace Apex.AI.Teaching
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Pathfinder
    {
        private const int maxIterations = 1000;

        private List<PathNode> _visited = new List<PathNode>(40);
        private List<PathNode> _openNodes = new List<PathNode>(40);

        private Cell _destinationCell;
        private PathNode _currentNode;
        private Path _lastPath;

        public Path FindPath(Grid grid, Vector3 start, Vector3 destination)
        {
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

            _destinationCell = destinationCell;
            _currentNode = new PathNode(null, startCell, 0f);
            _visited.Add(_currentNode);

            var counter = 0;
            while (Application.isPlaying)
            {
                if (FindPathIteration())
                {
                    break;
                }

                if (counter++ > maxIterations)
                {
                    Debug.LogWarning(this.ToString() + " could not find a path. Max tries exceeded");
                    break;
                }
            }

            Reset();
            return _lastPath;
        }

        private bool FindPathIteration()
        {
            var neighbours = _currentNode.cell.neighbours;
            var count = neighbours.Count;
            if (count == 0)
            {
                // Path not found
                Debug.LogWarning(this.ToString() + " could not find a path. No neighbours to start cell found");
                return true;
            }

            for (int i = 0; i < count; i++)
            {
                var neighbour = neighbours[i];
                if (ReferenceEquals(neighbour, _destinationCell))
                {
                    // Path found!
                    BuildPath();
                    return true;
                }

                if (HasVisited(neighbour))
                {
                    continue;
                }

                var cost = GetCost(neighbour, _currentNode.cell);
                var node = new PathNode(_currentNode, neighbour, cost);
                _openNodes.Add(node);
            }

            var openNodesCount = _openNodes.Count;
            var lowestCost = float.MaxValue;
            for (int j = 0; j < openNodesCount; j++)
            {
                var node = _openNodes[j];
                var cost = node.cost + GetCost(node.cell, _destinationCell);
                if (cost < lowestCost)
                {
                    lowestCost = cost;
                    _currentNode = node;
                }
            }

            _openNodes.Remove(_currentNode);
            _visited.Add(_currentNode);

            // Still iterating
            return false;
        }

        private void BuildPath()
        {
            var path = new Path(_visited.Count);
            var current = new PathNode(_currentNode, _destinationCell, 0f);
            while (current != null)
            {
                path.Add(current.position);
                current = current.parent;
            }

            path.Reverse();
            _lastPath = path;
        }

        private void Reset()
        {
            _currentNode = null;
            _destinationCell = null;
            _visited.Clear();
            _openNodes.Clear();
        }

        private bool HasVisited(Cell cell)
        {
            var count = _visited.Count;
            for (int i = 0; i < count; i++)
            {
                if (ReferenceEquals(_visited[i].cell, cell))
                {
                    return true;
                }
            }

            return false;
        }

        private float GetCost(Cell fromCell, Cell toCell)
        {
            return (toCell.position - fromCell.position).magnitude + (_destinationCell.position - fromCell.position).magnitude;
        }
    }
}