namespace Apex.AI.Teaching
{
    using UnityEngine;

    [RequireComponent(typeof(UnitBase))]
    [RequireComponent(typeof(SteerableUnit))]
    [RequireComponent(typeof(SteeringScanner))]
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

        /// <summary>
        /// Gets the priority of this particular steering component. The priority controls whether this steering component is executed. Higher priority steering components get executed first, and the first one with a value is used.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int priority
        {
            get { return _priority; }
        }

        private void OnEnable()
        {
            // Get references to unit and the steering scanner
            _unit = this.GetComponent<UnitBase>();
            _scanner = this.GetComponent<SteeringScanner>();
        }

        public Vector3? GetSteering(SteeringInput input)
        {
            if (!this.enabled || !this.gameObject.activeSelf)
            {
                // If this component or this game object is disabled, don't do steering
                return null;
            }

            var otherUnits = _scanner.units;
            var count = otherUnits.Count;
            if (count == 0)
            {
                // no scanned units to avoid
                return null;
            }

            var steeringVector = Vector3.zero;
            var separationCount = 0;
            for (int i = 0; i < count; i++)
            {
                var other = otherUnits[i];
                var combinedRadii = other.unitRadius + _unit.unitRadius + this.separationDistance;
                var direction = (_unit.transform.position - other.transform.position);
                if (direction.sqrMagnitude > (combinedRadii * combinedRadii))
                {
                    // units do not overlap => the distance between them is more than their combined radii + the desired separation distance
                    continue;
                }

                // Get the actual length of the vector from the unit to the other
                var mag = direction.magnitude;

                // Count up how many units we are separating from, and accumulate each "repulsion" vector
                separationCount++;
                steeringVector += (direction / mag) * (combinedRadii - mag);
            }

            if (separationCount == 0)
            {
                // No units to separate from
                return null;
            }

            // Divide by the separation count to 'average' the steering vector out between all the different influences
            steeringVector /= separationCount;
            if (steeringVector.sqrMagnitude <= 0f)
            {
                return null;
            }
            
            return steeringVector.normalized * input.speed;
        }
    }
}