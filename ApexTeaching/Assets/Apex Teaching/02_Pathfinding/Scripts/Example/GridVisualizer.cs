﻿namespace Apex.AI.Teaching
{
    using UnityEngine;

    public class GridVisualizer : MonoBehaviour
    {
        public bool drawAlways;
        public float colorAlpha = 0.3f;

        private Grid _grid;

        private void OnEnable()
        {
            _grid = this.GetComponent<Grid>();
        }

        private void OnDrawGizmos()
        {
            if (!this.enabled || !Application.isPlaying || _grid == null || _grid.cells.Length == 0 || !this.drawAlways)
            {
                return;
            }

            Draw();
        }

        private void OnDrawGizmosSelected()
        {
            if (!this.enabled || !Application.isPlaying || _grid == null || _grid.cells.Length == 0 || this.drawAlways)
            {
                return;
            }

            Draw();
        }

        private void Draw()
        {
            foreach (var cell in _grid.cells)
            {
                var color = cell.blocked ? Color.red : Color.green;
                color.a = this.colorAlpha;
                Gizmos.color = color;
                Gizmos.DrawCube(cell.position, cell.size);
            }
        }
    }
}