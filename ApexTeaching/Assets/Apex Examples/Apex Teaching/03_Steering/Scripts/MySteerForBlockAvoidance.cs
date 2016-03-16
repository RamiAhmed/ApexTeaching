#pragma warning disable 0414

namespace Apex.AI.Teaching
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(UnitBase))]
    [RequireComponent(typeof(SteerableUnit))]
    [RequireComponent(typeof(SteeringScanner))]
    public sealed class MySteerForBlockAvoidance : MonoBehaviour, ISteeringComponent
    {
        [SerializeField]
        private int _priority = 16;

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
            // TODO: Add implementation here (and return something different from null!)
            return null;
        }
    }
}