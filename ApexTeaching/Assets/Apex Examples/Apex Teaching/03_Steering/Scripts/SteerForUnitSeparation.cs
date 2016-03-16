namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class SteerForUnitSeparation : MonoBehaviour, ISteeringComponent
    {
        /// <summary>
        /// The separation distance - i.e. the desired distance between the extents of any two units
        /// </summary>
        public float separationDistance = 0.5f;

        [SerializeField]
        private int _priority = 5;

        private UnitBase _unit;
        private SteeringScanner _scanner;

        public int priority
        {
            get { return _priority; }
        }

        private void OnEnable()
        {
            _unit = this.GetComponent<UnitBase>();
            _scanner = this.GetComponent<SteeringScanner>();
        }

        public Vector3? GetSteering(SteeringInput input)
        {
            if (!this.enabled || !this.gameObject.activeSelf)
            {
                return null;
            }

            var otherUnits = _scanner.units;
            var count = otherUnits.Count;
            if (count == 0)
            {
                // no observations to avoid
                return null;
            }

            var steeringVector = Vector3.zero;
            var avoidedCount = 0;
            for (int i = 0; i < count; i++)
            {
                var other = otherUnits[i];
                var combinedRadii = other.unitRadius + _unit.unitRadius + this.separationDistance;
                var direction = (_unit.transform.position - other.transform.position);
                if (direction.sqrMagnitude >= (combinedRadii * combinedRadii))
                {
                    // units do not overlap
                    continue;
                }

                var mag = direction.magnitude;
                if (mag == 0f)
                {
                    continue;
                }

                avoidedCount++;
                steeringVector += (direction / mag) * (combinedRadii - mag);
            }

            if (avoidedCount == 0)
            {
                return null;
            }

            steeringVector /= (float)avoidedCount;
            if (steeringVector.sqrMagnitude <= 0f)
            {
                return null;
            }

            var dir = (this.transform.position + steeringVector) - this.transform.position;
            return dir.normalized * input.speed;
        }
    }
}