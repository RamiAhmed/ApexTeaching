namespace Apex.AI.Teaching
{
    using UnityEngine;

    public class PathNode
    {
        public PathNode parent;
        public Vector3 position;
        public Cell cell;
        public float cost;

        public PathNode(PathNode parent, Cell cell, float cost)
        {
            this.parent = parent;
            this.cell = cell;
            this.position = cell.position;
            this.cost = cost;
        }
    }
}