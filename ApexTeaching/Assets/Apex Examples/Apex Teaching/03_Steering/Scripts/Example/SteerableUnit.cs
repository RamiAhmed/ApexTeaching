namespace Apex.AI.Teaching
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(UnitBase))]
    public sealed class SteerableUnit : MonoBehaviour
    {
        public float speed = 6f;
        public float maxSpeed = 10f;
        public float angularSpeed = 5f;

        private ISteeringComponent[] _steeringComponents;
        private UnitBase _unit;

        private void Awake()
        {
            // Get all steering components (Implementing ISteeringComponent) and sort them descending based on priority
            _steeringComponents = this.GetComponents<ISteeringComponent>();
            Array.Sort(_steeringComponents, (a, b) =>
            {
                return b.priority.CompareTo(a.priority);
            });

            // Get the unit's component
            _unit = this.GetComponent<UnitBase>();
        }

        private void FixedUpdate()
        {
            // Create the input object
            var input = new SteeringInput()
            {
                speed = this.speed,
                maxSpeed = this.maxSpeed,
                angularSpeed = this.angularSpeed
            };

            // iterate through all identified steering components
            var steering = Vector3.zero;
            for (int i = 0; i < _steeringComponents.Length; i++)
            {
                var steer = _steeringComponents[i].GetSteering(input);
                if (steer.HasValue)
                {
                    // if a steering component has output, use it's output value and stop iterating (only one component active at a time)
                    steering = steer.Value;
                    break;
                }
            }

            if (steering.sqrMagnitude == 0f)
            {
                // No steering vector
                return;
            }

            // The steering vector is the velocity, clamped to the maximum speed. However, the y-component is ignored to avoid movements in Y-axis
            var velocity = Vector3.ClampMagnitude(steering, this.maxSpeed);
            velocity.y = 0f;
            _unit.velocity = velocity;

            // Apply the steering
            this.transform.position += velocity * Time.fixedDeltaTime;

            if (velocity != Vector3.zero)
            {
                // Simply generate a quaternion for facing in the direction that the unit is moving, using the Y-axis as 'Up' - and smoothly rotate to the new rotation
                var rotation = Quaternion.LookRotation(velocity, Vector3.up);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.fixedDeltaTime * this.angularSpeed);
            }
        }
    }
}