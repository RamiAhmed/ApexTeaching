namespace Apex.AI.Teaching
{
    using System;
    using UnityEngine;

    public sealed class SteerableUnit : MonoBehaviour
    {
        public float speed = 6f;
        public float maxSpeed = 10f;
        public float angularSpeed = 5f;

        private ISteeringComponent[] _steeringComponents;
        private UnitBase _unit;

        private void Awake()
        {
            _steeringComponents = this.GetComponents<ISteeringComponent>();
            Array.Sort(_steeringComponents, (a, b) =>
            {
                return b.priority.CompareTo(a.priority);
            });

            _unit = this.GetComponent<UnitBase>();
        }

        private void FixedUpdate()
        {
            var input = new SteeringInput()
            {
                speed = this.speed,
                maxSpeed = this.maxSpeed,
                angularSpeed = this.angularSpeed
            };

            var steering = Vector3.zero;
            for (int i = 0; i < _steeringComponents.Length; i++)
            {
                var steer = _steeringComponents[i].GetSteering(input);
                if (steer.HasValue)
                {
                    steering += steer.Value;
                    break;
                }
            }

            if (steering.sqrMagnitude == 0f)
            {
                return;
            }

            var velocity = _unit.velocity = Vector3.ClampMagnitude(steering, this.maxSpeed);
            this.transform.position += velocity * Time.fixedDeltaTime;

            // Simply generate a quaternion for facing in the direction that the unit is moving, using the Y-axis as 'Up' - and smoothly rotate to the new rotation
            if (velocity != Vector3.zero)
            {
                var rotation = Quaternion.LookRotation(velocity, Vector3.up);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.fixedDeltaTime * this.angularSpeed);
            }
        }
    }
}