namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class SteeringVisualizer : MonoBehaviour
    {
        public bool drawAlways;
        public Color gizmosColor = Color.magenta;

        private UnitBase _unit;

        private void OnEnable()
        {
            _unit = this.GetComponent<UnitBase>();
        }

        private void OnDrawGizmos()
        {
            if (!this.enabled || !Application.isPlaying || _unit == null || !this.drawAlways)
            {
                return;
            }

            Visualize();
        }

        private void OnDrawGizmosSelected()
        {
            if (!this.enabled || !Application.isPlaying || _unit == null || this.drawAlways)
            {
                return;
            }

            Visualize();
        }

        private void Visualize()
        {
            Gizmos.color = this.gizmosColor;

            var velocity = _unit.velocity;
            Gizmos.DrawLine(this.transform.position, this.transform.position + velocity);

            var radius = _unit.unitRadius;
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }
}