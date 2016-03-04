#pragma warning disable 0414

namespace Apex.AI.Teaching
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class UnitBase : MonoBehaviour, ICanDie
    {
        [SerializeField]
        private ParticleSystem _deathEffect;

        [SerializeField]
        protected float _maxHealth = 100f;

        [SerializeField]
        protected float _attackRadius = 2f;

        [SerializeField]
        protected float _attacksPerSecond = 1f;

        [SerializeField]
        private float _minDamage = 10f;

        [SerializeField]
        private float _maxDamage = 20f;

        [SerializeField]
        private float _scanRadius = 15f;

        [SerializeField]
        private float _randomWanderRadius = 10f;

        protected float _lastAttack;
        protected List<GameObject> _observations;
        private NavMeshAgent _navMeshAgent;

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

        private void Awake()
        {
            _navMeshAgent = this.GetComponent<NavMeshAgent>();
            _navMeshAgent.avoidancePriority += Random.Range(-23, 24);
        }

        private void OnEnable()
        {
            _observations = new List<GameObject>(5);
            this.currentHealth = _maxHealth;
        }

        private void OnDisable()
        {
            if (this.nest != null)
            {
                this.nest.ReturnUnit(this);
            }
        }

        protected float GetDamage()
        {
            return Random.Range(_minDamage, _maxDamage);
        }

        public void AddOrUpdateObservations(Collider[] colliders)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                AddOrUpdateObservationInternal(colliders[i].gameObject);
            }
        }

        private void AddOrUpdateObservationInternal(GameObject gameObject)
        {
            var idx = _observations.IndexOf(gameObject);
            if (idx >= 0)
            {
                _observations[idx] = gameObject;
                return;
            }

            _observations.Add(gameObject);
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
            var randomPos = this.transform.position + Random.onUnitSphere.normalized * _randomWanderRadius;
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
                PlayEffect(_deathEffect);
                this.gameObject.SetActive(false);
            }
        }

        protected void PlayEffect(ParticleSystem effect)
        {
            effect.transform.SetParent(null);
            effect.Play();
            CoroutineHelper.instance.StartCoroutine(CleanUpEffect(effect));
        }

        private IEnumerator CleanUpEffect(ParticleSystem effect)
        {
            yield return new WaitForSeconds(effect.duration);
            effect.transform.SetParent(this.transform);
        }

        public void Attack()
        {
            var time = Time.time;
            if (time - _lastAttack < 1f / _attacksPerSecond)
            {
                return;
            }

            _lastAttack = time;
            StopMoving();
            InternalAttack(GetDamage());
        }

        protected abstract void InternalAttack(float dmg);
    }
}