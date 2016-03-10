namespace Apex.AI.Teaching
{
    using UnityEngine;

    [RequireComponent(typeof(UnitBase))]
    public sealed class DestinationVisualizer : MonoBehaviour
    {
        public Color gizmosColor = Color.green;
        public float sphereSize = 0.5f;

        private UnitBase _unit;
        private SteerForPath _steerForPath;

        private void OnEnable()
        {
            _unit = this.GetComponent<UnitBase>();
            _steerForPath = _unit.GetComponent<SteerForPath>();
        }

        private void OnDrawGizmosSelected()
        {
            if (!this.enabled || !Application.isPlaying)
            {
                return;
            }

            var navMeshAgent = _unit.navMeshAgent;
            if (navMeshAgent != null)
            {
                var destination = navMeshAgent.destination;
                if (destination.sqrMagnitude > 0f)
                {
                    Gizmos.color = this.gizmosColor;
                    Gizmos.DrawSphere(destination, this.sphereSize);
                    Gizmos.DrawLine(this.transform.position, destination);
                }

                return;
            }

            if (_steerForPath != null)
            {
                var path = _steerForPath.path;
                if (path != null)
                {
                    var count = path.Count;
                    if (count > 0)
                    {
                        Gizmos.color = this.gizmosColor;
                        for (int i = 0; i < count - 1; i++)
                        {
                            Gizmos.DrawSphere(path[i], this.sphereSize);
                            Gizmos.DrawLine(path[i], path[i + 1]);
                        }

                        Gizmos.DrawSphere(path.last, this.sphereSize);
                    }
                }
            }
        }
    }
}