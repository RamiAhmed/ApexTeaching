namespace Apex.AI.Teaching
{
    using System;
    using UnityEngine;

    public sealed class HarvesterUnit : UnitBase
    {
        [SerializeField]
        private int _maxCarriableResources = 25;

        [SerializeField]
        private int _currentCarriedResources = 0;

        [SerializeField]
        private float _fleeRadius = 10f;

        [SerializeField]
        private float _returnHarvestRadius = 4f;

        public override UnitType type
        {
            get { return UnitType.Harvester; }
        }

        public float returnHarvestRadius
        {
            get { return _returnHarvestRadius; }
        }

        public float fleeRadius
        {
            get { return _fleeRadius; }
        }

        public int currentCarriedResources
        {
            get { return _currentCarriedResources; }
            set { _currentCarriedResources = Mathf.Min(value, _maxCarriableResources); }
        }

        public int maxCarriableResources
        {
            get { return _maxCarriableResources; }
        }

        protected override void InternalAttack(float dmg)
        {
            // harvesters hit only one unit or nest
            var hits = Physics.OverlapSphere(this.transform.position, _attackRadius, Layers.units | Layers.structures);
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];

                var unit = hit.GetComponent(typeof(UnitBase)) as UnitBase;
                if (unit != null)
                {
                    unit.ReceiveDamage(dmg);
                    return;
                }

                var nest = hit.GetComponent<NestStructure>();
                if (nest != null)
                {
                    nest.ReceiveDamage(dmg);
                    return;
                }
            }
        }

        public void Harvest(ResourceComponent resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            var time = Time.time;
            if (time - _lastAttack < 1f / _attacksPerSecond)
            {
                return;
            }

            _lastAttack = time;
            resource.Harvest(this);
        }
    }
}