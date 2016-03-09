namespace Apex.AI.Teaching
{
    using System;
    using UnityEngine;

    public class HarvesterUnit : UnitBase
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

        /// <summary>
        /// Gets the return harvest radius - the radius within which this unit may return harvested resources to the nest.
        /// </summary>
        /// <value>
        /// The return harvest radius.
        /// </value>
        public float returnHarvestRadius
        {
            get { return _returnHarvestRadius; }
        }

        /// <summary>
        /// Gets the flee radius - if it observes another unit within this range, it will attempt to flee.
        /// </summary>
        /// <value>
        /// The flee radius.
        /// </value>
        public float fleeRadius
        {
            get { return _fleeRadius; }
        }

        /// <summary>
        /// Gets the current amount of carried resources - DO NOT MODIFY.
        /// </summary>
        /// <value>
        /// The current carried resources.
        /// </value>
        public int currentCarriedResources
        {
            get { return _currentCarriedResources; }
            set { _currentCarriedResources = Mathf.Min(value, _maxCarriableResources); }
        }

        /// <summary>
        /// Gets the maximum carriable resources.
        /// </summary>
        /// <value>
        /// The maximum carriable resources.
        /// </value>
        public int maxCarriableResources
        {
            get { return _maxCarriableResources; }
        }

        protected override void InternalAttack(float dmg)
        {
            // harvesters hit only one unit or nest
            var hits = Physics.OverlapSphere(this.transform.position, _attackRadius, Layers.mortal);
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (ReferenceEquals(hit.gameObject, this.gameObject))
                {
                    // don't damage self
                    continue;
                }

                var unit = hit.GetComponent<UnitBase>();
                if (unit != null)
                {
                    unit.ReceiveDamage(dmg);
                    this.transform.LookAt(new Vector3(unit.transform.position.x, this.transform.position.y, unit.transform.position.z));
                    return;
                }

                var nest = hit.GetComponent<NestStructure>();
                if (nest != null)
                {
                    nest.ReceiveDamage(dmg);
                    this.transform.LookAt(new Vector3(unit.transform.position.x, this.transform.position.y, unit.transform.position.z));
                    return;
                }
            }
        }

        /// <summary>
        /// Harvests the specified resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <exception cref="ArgumentNullException">resource</exception>
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