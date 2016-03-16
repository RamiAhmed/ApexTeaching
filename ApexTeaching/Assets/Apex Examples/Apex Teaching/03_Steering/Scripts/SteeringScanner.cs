namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class SteeringScanner : MonoBehaviour
    {
        /// <summary>
        /// The scan radius, i.e. how large a radius units are picked up in.
        /// </summary>
        public float scanRadius = 10f;

        /// <summary>
        /// The scan interval, i.e. how often steering scanning is executed.
        /// </summary>
        public float scanInterval = 0.2f;

        private Collider[] _colliders = new Collider[0];
        private float _lastScan;

        public List<UnitBase> units
        {
            get;
            private set;
        }

        private void OnEnable()
        {
            this.units = new List<UnitBase>(2);
        }

        private void Update()
        {
            var time = Time.time;
            if (time - _lastScan < this.scanInterval)
            {
                return;
            }

            _lastScan = time;
            this.units.Clear();

            _colliders = Physics.OverlapSphere(this.transform.position, this.scanRadius, Layers.units);
            if (_colliders.Length == 0)
            {
                return;
            }

            for (int i = 0; i < _colliders.Length; i++)
            {
                var coll = _colliders[i];
                if (coll == null)
                {
                    // ignore null entries
                    continue;
                }

                if (ReferenceEquals(coll.gameObject, this.gameObject))
                {
                    // Do not record 'self'
                    continue;
                }

                var unitComp = coll.GetComponent<UnitBase>();
                if (unitComp == null)
                {
                    // Not a unit? Ignore it
                    continue;
                }

                this.units.Add(unitComp);
            }

            if (this.units.Count == 0)
            {
                return;
            }

            // Sort so that nearest are first
            this.units.Sort(new UnitBaseDistanceSortComparer(this.transform.position));
        }
    }
}