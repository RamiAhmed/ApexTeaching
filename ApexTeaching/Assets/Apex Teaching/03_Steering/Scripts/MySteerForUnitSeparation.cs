#pragma warning disable 0414

namespace Apex.AI.Teaching
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(UnitBase))]
    [RequireComponent(typeof(SteerableUnit))]
    public sealed class MySteerForUnitSeparation : MonoBehaviour, ISteeringComponent
    {
        [SerializeField]
        private int _priority = 11;

        private UnitBase _unit;

        public int priority
        {
            get { return _priority; }
        }

        private void OnEnable()
        {
            _unit = this.GetComponent<UnitBase>();
        }

        public Vector3? GetSteering(SteeringInput input)
        {
            // TODO: Add implementation here (and return something different from null!)
            return null;
        }
    }
}