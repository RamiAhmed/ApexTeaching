#pragma warning disable 0414

namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    public abstract class UnitBase : MonoBehaviour, ICanDie
    {
        [SerializeField]
        protected float _maxHealth = 100f;

        [SerializeField]
        protected float _minDamage = 10f;

        [SerializeField]
        protected float _maxDamage = 20f;

        [SerializeField]
        protected float _attackRadius = 2f;

        [SerializeField]
        protected float _attacksPerSecond = 1f;

        [SerializeField]
        protected float _scanRadius = 15f;

        [SerializeField]
        protected float _randomWanderRadius = 10f;

        protected NavMeshAgent _navMeshAgent;
        protected float _lastAttack;
        protected List<GameObject> _observations;

        public abstract UnitType type { get; }

        public int id
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

        public float attackRadius
        {
            get { return _attackRadius; }
        }

        public float scanRadius
        {
            get { return _scanRadius; }
        }

        public NestStructure nest
        {
            get;
            set;
        }

        public List<GameObject> observations
        {
            get { return _observations; }
        }

        public NavMeshAgent navMeshAgent
        {
            get { return _navMeshAgent; }
        }

        public bool isMoving
        {
            get { return _navMeshAgent.desiredVelocity.sqrMagnitude > 1f; }
        }

        protected virtual void Awake()
        {
            _navMeshAgent = this.GetComponent<NavMeshAgent>();
            _navMeshAgent.avoidancePriority += Random.Range(-23, 24);
        }

        protected virtual void OnEnable()
        {
            _observations = new List<GameObject>(5);
            this.currentHealth = _maxHealth;
        }

        protected virtual void OnDisable()
        {
            if (this.nest != null)
            {
                this.nest.ReturnUnit(this);
            }
        }

        private float GetDamage()
        {
            return Random.Range(_minDamage, _maxDamage);
        }

        public void AddOrUpdateObservations(Collider[] colliders)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                AddOrUpdateObservation(colliders[i].gameObject);
            }
        }

        public void AddOrUpdateObservation(GameObject gameObject)
        {
            var idx = _observations.IndexOf(gameObject);
            if (idx >= 0)
            {
                _observations[idx] = gameObject;
                return;
            }

            _observations.Add(gameObject);
        }

        public bool RemoveObservation(GameObject gameObject)
        {
            return _observations.Remove(gameObject);
        }

        public bool IsAllied(UnitBase otherUnit)
        {
            return ReferenceEquals(this.nest, otherUnit.nest);
        }

        public bool IsAllied(NestStructure nest)
        {
            return ReferenceEquals(this.nest, nest);
        }

        public void RandomWander()
        {
            var randomPos = this.transform.position + Random.onUnitSphere.normalized * this._randomWanderRadius;
            randomPos.y = this.transform.position.y;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, _randomWanderRadius * 0.5f, _navMeshAgent.areaMask))
            {
                MoveTo(hit.position);
            }
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
        }

        public void StopMoving()
        {
            _navMeshAgent.Stop();
        }

        public void ReceiveDamage(float dmg)
        {
            this.currentHealth -= dmg;
            if (this.currentHealth <= 0f)
            {
                this.gameObject.SetActive(false);
            }
        }

        public void Attack()
        {
            var time = Time.time;
            if (time - _lastAttack < 1f / _attacksPerSecond)
            {
                return;
            }

            _lastAttack = time;
            InternalAttack(GetDamage());
        }

        protected abstract void InternalAttack(float dmg);
    }
}