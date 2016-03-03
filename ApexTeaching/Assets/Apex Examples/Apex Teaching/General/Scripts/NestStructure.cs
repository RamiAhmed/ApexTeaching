namespace Apex.AI.Teaching
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class NestStructure : MonoBehaviour, ICanDie
    {
        [SerializeField]
        private float _maxHealth = 1000f;

        [SerializeField]
        private float _spawnDistance = 5f;

        [SerializeField]
        private float _buildCooldown = 0.5f;

        [SerializeField]
        private int _startHarvesters = 3;

        [SerializeField]
        private int _initialInstanceCount = 30;

        [SerializeField]
        private GameObject _harvesterPrefab;

        [SerializeField]
        private GameObject _exploderPrefab;

        [SerializeField]
        private GameObject _fighterPrefab;

        private Dictionary<UnitType, UnitPool> _entityPools;
        private float _lastBuild;

        public AIController controller
        {
            get;
            set;
        }

        public float maxHealth
        {
            get { return _maxHealth; }
        }

        public float currentHealth
        {
            get;
            private set;
        }

        public bool isDead
        {
            get { return this.currentHealth <= 0f; }
        }

        private void Awake()
        {
            _entityPools = new Dictionary<UnitType, UnitPool>(new UnitTypeComparer())
            {
                { UnitType.Harvester, new UnitPool(this._harvesterPrefab, this.gameObject, this._initialInstanceCount) },
                { UnitType.Exploder, new UnitPool(this._exploderPrefab, this.gameObject, this._initialInstanceCount) },
                { UnitType.Fighter, new UnitPool(this._fighterPrefab, this.gameObject, this._initialInstanceCount) }
            };
        }

        private void OnEnable()
        {
            this.currentHealth = _maxHealth;
            StartCoroutine(BuildInitialHarvesters());
        }

        private void OnDisable()
        {
            var units = this.controller.units;
            var count = units.Count;
            for (int i = 0; i < count; i++)
            {
                ReturnUnit(units[i]);
            }
        }

        private IEnumerator BuildInitialHarvesters()
        {
            yield return new WaitForSeconds(1f);

            if (this._startHarvesters > 0)
            {
                for (int i = 0; i < this._startHarvesters; i++)
                {
                    InternalBuildUnit(UnitType.Harvester);
                }
            }
        }

        public void BuildUnit(UnitType type)
        {
            if (type == UnitType.None)
            {
                Debug.LogError(this.ToString() + " cannot build units of type 'None'");
                return;
            }

            var cost = UnitCostManager.GetCost(type);
            if (cost > this.controller.currentResources)
            {
                // AI cannot afford the unit
                return;
            }

            var time = Time.time;
            if (time - _lastBuild < _buildCooldown)
            {
                // cooldown still in effect
                return;
            }

            _lastBuild = time;
            this.controller.currentResources -= cost;
            InternalBuildUnit(type);
        }

        private void InternalBuildUnit(UnitType type)
        {
            var pos = this.transform.position + (UnityEngine.Random.onUnitSphere.normalized * _spawnDistance);
            pos.y = this.transform.position.y;

            var unit = _entityPools[type].Get(pos, Quaternion.identity);
            unit.nest = this;

            // color unit
            var color = this.controller.color;
            var renderers = unit.GetComponents<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = color;
            }

            renderers = unit.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = color;
            }

            this.controller.units.Add(unit);
        }

        public void ReturnUnit(UnitBase unit)
        {
            if (unit.type == UnitType.None)
            {
                Debug.LogError(this.ToString() + " cannot return units of type 'None'");
                return;
            }

            this.controller.units.Remove(unit);
            _entityPools[unit.type].Return(unit);
        }

        public void ReceiveDamage(float dmg)
        {
            this.currentHealth -= dmg;
            if (this.currentHealth <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}