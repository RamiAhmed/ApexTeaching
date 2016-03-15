namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class SteerForUnitAvoidance : MonoBehaviour, ISteeringComponent
    {
        public float radiusMargin = 0.1f;
        public int priority = 10;
        public float fieldOfView = 200f;

        private UnitBase _unit;
        private SteeringScanner _scanner;
        private float _fovReverseAngleCos;
        private float _fovHeadOnAngleCos;

        private Vector3 _lastAvoidVector;

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        int ISteeringComponent.priority
        {
            get { return priority; }
        }

        private void OnEnable()
        {
            _unit = this.GetComponent<UnitBase>();
            _scanner = this.GetComponent<SteeringScanner>();
            _fovReverseAngleCos = Mathf.Cos(((360f - this.fieldOfView) / 2f) * Mathf.Deg2Rad);
            _fovHeadOnAngleCos = Mathf.Cos(140f * Mathf.Deg2Rad);
        }

        public Vector3? GetSteering(SteeringInput input)
        {
            _lastAvoidVector = Vector3.zero;

            var otherUnits = _scanner.units;
            var count = otherUnits.Count;
            if (count == 0)
            {
                // no observations to avoid
                return null;
            }

            var velocityNorm = _unit.velocity.normalized;
            for (int i = 0; i < count; i++)
            {
                var otherUnit = otherUnits[i];

                var dir = (this.transform.position - otherUnit.transform.position);
                var mag = dir.magnitude;
                if (mag > (_unit.unitRadius * 2f) && Vector3.Dot(velocityNorm, dir / mag) > _fovReverseAngleCos)
                {
                    // the other unit is behind me and outside my 'omni aware radius', ignore it
                    continue;
                }

                var collisionDistance = 0f;
                var avoidDir = GetAvoidVector(otherUnit, out collisionDistance);
                if (!avoidDir.HasValue)
                {
                    // no collision imminent
                    continue;
                }

                var avoidMagnitude = avoidDir.Value.magnitude;
                if (avoidMagnitude == 0f)
                {
                    continue;
                }

                // half of combined radii is vector length
                var vectorLength = (otherUnit.unitRadius + _unit.unitRadius + this.radiusMargin) * 0.5f;
                if (vectorLength <= 0f)
                {
                    continue;
                }

                var avoidNormalized = avoidDir.Value / avoidMagnitude;
                var avoidVector = avoidNormalized * vectorLength;

                var dotAngle = Vector3.Dot(avoidNormalized, velocityNorm);
                if (dotAngle <= _fovHeadOnAngleCos)
                {
                    // the collision is considered "head-on", thus we compute a perpendicular avoid vector instead
                    avoidVector = new Vector3(avoidVector.z, avoidVector.y, -avoidVector.x);
                }

                if (avoidVector.sqrMagnitude < 0.1f)
                {
                    continue;
                }

                //avoidVector *= (_unit.velocity.magnitude / collisionDistance);
                avoidVector = Vector3.ClampMagnitude(avoidVector, input.speed);
                _lastAvoidVector = avoidVector;
                return avoidVector;
            }

            return null;
        }

        private Vector3? GetAvoidVector(UnitBase otherUnit, out float collisionDistance)
        {
            collisionDistance = 0f;

            // prepare variables
            var position = _unit.transform.position;
            var velocity = _unit.velocity;
            var otherPos = otherUnit.transform.position;
            var otherVelocity = otherUnit.velocity;
            var combinedRadii = _unit.unitRadius + otherUnit.unitRadius + this.radiusMargin;

            // use a 2nd degree polynomial function to determine intersection points between moving units with a velocity and radius
            var a = ((velocity.x - otherVelocity.x) * (velocity.x - otherVelocity.x)) +
                    ((velocity.z - otherVelocity.z) * (velocity.z - otherVelocity.z));
            var b = (2f * (position.x - otherPos.x) * (velocity.x - otherVelocity.x)) +
                    (2f * (position.z - otherPos.z) * (velocity.z - otherVelocity.z));
            var c = ((position.x - otherPos.x) * (position.x - otherPos.x)) +
                    ((position.z - otherPos.z) * (position.z - otherPos.z)) -
                    (combinedRadii * combinedRadii);

            var d = (b * b) - (4f * a * c);
            if (d <= 0f)
            {
                // there are not 2 intersection points
                return null;
            }

            var dSqrt = Mathf.Sqrt(d);
            var doubleA = 2f * a;

            // compute roots, which in this case are actually time values informing of when the collision starts and ends
            var t1 = (-b * dSqrt) / doubleA;
            var t2 = (-b - dSqrt) / doubleA;

            if (t1 < 0f && t2 < 0f)
            {
                // if both times are negative, the collision is behind us (compared to velocity direction)
                return null;
            }

            // find the lowest non-negative time, since this will be where the collision time interval starts
            float time = 0f;
            if (t1 < 0f)
            {
                time = t2;
            }
            else if (t2 < 0f)
            {
                time = t1;
            }
            else
            {
                time = Mathf.Min(t1, t2);
            }

            // the collision time we want is actually 25 % within the collision
            time += Mathf.Abs(t2 - t1) * 0.25f;

            // compute actual collision positions
            var selfCollisionPos = position + (velocity * time);
            var otherCollisionPos = otherPos + (otherVelocity * time);

            // scale the avoid vector depending on the distance to collision, shorter distances need larger magnitudes and vice versa
            collisionDistance = Mathf.Max(1f, (selfCollisionPos - position).magnitude);
            return (selfCollisionPos - otherCollisionPos);
        }

        private void OnDrawGizmosSelected()
        {
            if (!this.enabled || !Application.isPlaying || _unit == null)
            {
                return;
            }

            if (_lastAvoidVector.sqrMagnitude == 0f)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, this.transform.position + _lastAvoidVector);
            Gizmos.DrawSphere(this.transform.position + _lastAvoidVector, 0.25f);
        }
    }
}