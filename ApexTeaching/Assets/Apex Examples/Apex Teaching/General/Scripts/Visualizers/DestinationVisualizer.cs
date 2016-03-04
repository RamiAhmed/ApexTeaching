namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class DestinationVisualizer : MonoBehaviour
    {
        public Color gizmosColor = Color.green;
        public float sphereSize = 0.5f;

        private UnitBase _unit;

        private void OnEnable()
        {
            _unit = this.GetComponent<UnitBase>();
        }

        private void OnDrawGizmosSelected()
        {
            if (!this.enabled || !Application.isPlaying)
            {
                return;
            }

            var navMeshAgent = _unit.navMeshAgent;
            var destination = navMeshAgent.destination;
            Gizmos.color = this.gizmosColor;
            Gizmos.DrawSphere(destination, this.sphereSize);
            Gizmos.DrawLine(this.transform.position, destination);
        }
    }
}