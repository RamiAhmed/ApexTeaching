namespace Apex.AI.Teaching
{
    using UnityEngine;

    [RequireComponent(typeof(UnitBase))]
    public sealed class ObservationsVisualizer : MonoBehaviour
    {
        public Color gizmosColor = Color.grey;
        public float sphereSize = 1f;

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

            var observations = _unit.observations;
            var count = observations.Count;
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                Gizmos.color = this.gizmosColor;
                Gizmos.DrawSphere(obs.transform.position, this.sphereSize);
                Gizmos.DrawLine(this.transform.position, obs.transform.position);
            }
        }
    }
}