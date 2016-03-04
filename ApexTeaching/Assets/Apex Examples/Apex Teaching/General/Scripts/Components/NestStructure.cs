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
        private float _spawnDistance = 10f;

        [SerializeField]
        private float _buildCooldown = 0.5f;

        [SerializeField]
        private int _startHarvesters = 3;

        [SerializeField]
        private int _startWarriors = 2;

        [SerializeField]
        private int _startBlasters = 1;

        [SerializeField]
        private int _initialInstanceCount = 30;

        [SerializeField]
        private GameObject _harvesterPrefab;

        [SerializeField]
        private GameObject _blasterPrefab;

        [SerializeField]
        private GameObject _warriorPrefab;

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

        public int harvesterCount
        {
            get { return _entityPools[UnitType.Harvester].count; }
        }

        public int warriorCount
        {
            get { return _entityPools[UnitType.Warrior].count; }
        }

        public int blasterCount
        {
            get { return _entityPools[UnitType.Blaster].count; }
        }

        private void Awake()
        {
            _entityPools = new Dictionary<UnitType, UnitPool>(new UnitTypeComparer())
            {
                { UnitType.Harvester, new UnitPool(_harvesterPrefab, this.gameObject, _initialInstanceCount) },
                { UnitType.Blaster, new UnitPool(_blasterPrefab, this.gameObject, _initialInstanceCount) },
                { UnitType.Warrior, new UnitPool(_warriorPrefab, this.gameObject, _initialInstanceCount) }
            };
        }

        private void OnEnable()
        {
            this.currentHealth = _maxHealth;
            StartCoroutine(BuildInitialUnits());
        }

        private void OnDisable()
        {
            var units = this.controller.units;
            var count = units.Count;
            for (int i = 0; i < count; i++)
            {
                // all the nest's units die when the nest dies
                units[i].ReceiveDamage(units[i].maxHealth + 1f);
                ReturnUnit(units[i]);
            }
        }

        private IEnumerator BuildInitialUnits()
        {
            if (_startHarvesters > 0)
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine(BuildUnitsGradually(_startHarvesters, UnitType.Harvester));
            }

            if (_startWarriors > 0)
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine(BuildUnitsGradually(_startWarriors, UnitType.Warrior));
            }

            if (_startBlasters > 0)
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine(BuildUnitsGradually(_startBlasters, UnitType.Blaster));
            }
        }

        private IEnumerator BuildUnitsGradually(int count, UnitType type)
        {
            for (int i = 0; i < count; i++)
            {
                InternalBuildUnit(type);
                yield return new WaitForSeconds(_buildCooldown);
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
            var renderers = unit.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].GetComponent<ParticleSystem>() == null)
                {
                    renderers[i].material.color = color;
                }
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