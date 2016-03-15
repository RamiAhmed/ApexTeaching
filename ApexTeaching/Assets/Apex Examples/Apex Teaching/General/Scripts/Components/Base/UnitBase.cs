#pragma warning disable 0414

namespace Apex.AI.Teaching
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class UnitBase : MonoBehaviour, ICanDie
    {
        private const float destinationBufferRadius = 2f;

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
        protected float _randomWanderRadius = 10f;

        [SerializeField]
        private float _currentHealth;

        [SerializeField]
        private float _unitRadius = 0.6f;

        protected float _lastAttack;
        protected List<GameObject> _observations;
        private NavMeshAgent _navMeshAgent;

        /// <summary>
        /// Gets the type of unit.
        /// </summary>
        /// <value>
        /// The type of unit.
        /// </value>
        public abstract UnitType type { get; }

        /// <summary>
        /// Gets the unique identifier used by the UnitPool - DO NOT MODIFY.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the maximum health.
        /// </summary>
        /// <value>
        /// The maximum health.
        /// </value>
        public float maxHealth
        {
            get { return _maxHealth; }
        }

        /// <summary>
        /// Gets the current health.
        /// </summary>
        /// <value>
        /// The current health.
        /// </value>
        public float currentHealth
        {
            get { return _currentHealth; }
        }

        /// <summary>
        /// Gets a value indicating whether this unit is dead.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this unit is dead; otherwise, <c>false</c>.
        /// </value>
        public bool isDead
        {
            get { return _currentHealth <= 0f || !this.gameObject.activeSelf; }
        }

        /// <summary>
        /// Gets the attack radius - the radius within which it can damage other units or nests.
        /// </summary>
        /// <value>
        /// The attack radius.
        /// </value>
        public float attackRadius
        {
            get { return _attackRadius; }
        }

        /// <summary>
        /// Gets the scan radius - the radius within which it can see other units, resources or nests.
        /// </summary>
        /// <value>
        /// The scan radius.
        /// </value>
        public float scanRadius
        {
            get { return _scanRadius; }
        }

        /// <summary>
        /// Gets a reference to the nest that spawned this unit - DO NOT MODIFY.
        /// </summary>
        /// <value>
        /// The nest.
        /// </value>
        public NestStructure nest
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a list of all the observations in memory for this unit.
        /// </summary>
        /// <value>
        /// The observations.
        /// </value>
        public List<GameObject> observations
        {
            get { return _observations; }
        }

        /// <summary>
        /// Gets a reference to the nav mesh agent used for navigation.
        /// </summary>
        /// <value>
        /// The nav mesh agent.
        /// </value>
        public NavMeshAgent navMeshAgent
        {
            get { return _navMeshAgent; }
        }

        /// <summary>
        /// Gets a value indicating whether this unit is moving.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this unit is moving; otherwise, <c>false</c>.
        /// </value>
        public virtual bool isMoving
        {
            get { return _navMeshAgent != null ? _navMeshAgent.desiredVelocity.sqrMagnitude > 1f : false; }
        }

        /// <summary>
        /// Gets the unit radius.
        /// </summary>
        /// <value>
        /// The unit radius.
        /// </value>
        public float unitRadius
        {
            get { return _unitRadius; }
        }

        /// <summary>
        /// Gets or sets the velocity.
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        public Vector3 velocity
        {
            get;
            set;
        }

        private void Awake()
        {
            _navMeshAgent = this.GetComponent<NavMeshAgent>();
            if (_navMeshAgent != null)
            {
                _navMeshAgent.avoidancePriority += Random.Range(-23, 24);
            }
        }

        private void OnEnable()
        {
            _observations = new List<GameObject>(5);
            _currentHealth = _maxHealth;
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

        /// <summary>
        /// Adds or updates an observation.
        /// </summary>
        /// <param name="colliders">The colliders observed.</param>
        public void AddOrUpdateObservations(Collider[] colliders)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                AddOrUpdateObservationInternal(colliders[i].gameObject);
            }
        }

        private void AddOrUpdateObservationInternal(GameObject gameObject)
        {
            if (ReferenceEquals(gameObject, this.gameObject))
            {
                return;
            }

            var idx = _observations.IndexOf(gameObject);
            if (idx >= 0)
            {
                _observations[idx] = gameObject;
                return;
            }

            _observations.Add(gameObject);
        }

        /// <summary>
        /// Determines whether the specified other unit is allied.
        /// </summary>
        /// <param name="otherUnit">The other unit.</param>
        /// <returns><c>true</c> if the other unit is allied; otherwise, <c>false</c>.</returns>
        public bool IsAllied(UnitBase otherUnit)
        {
            return ReferenceEquals(this.nest, otherUnit.nest);
        }

        /// <summary>
        /// Determines whether the specified nest is allied.
        /// </summary>
        /// <param name="nest">The nest.</param>
        /// <returns><c>true</c> if the other nest is allied; otherwise, <c>false</c>.</returns>
        public bool IsAllied(NestStructure nest)
        {
            return ReferenceEquals(this.nest, nest);
        }

        /// <summary>
        /// Makes this unit generate a random destination that it subsequently moves to
        /// </summary>
        public virtual void RandomWander()
        {
            var randomPos = this.transform.position + Random.onUnitSphere.normalized * _randomWanderRadius;
            randomPos.y = this.transform.position.y;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, _randomWanderRadius * 0.5f, _navMeshAgent.areaMask))
            {
                MoveTo(hit.position);
            }
        }

        /// <summary>
        /// Moves to a specified destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        public virtual void MoveTo(Vector3 destination)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(destination, out hit, destinationBufferRadius, _navMeshAgent.areaMask))
            {
                _navMeshAgent.Resume();
                _navMeshAgent.SetDestination(hit.position);
            }
        }

        /// <summary>
        /// Stops all movement.
        /// </summary>
        public virtual void StopMoving()
        {
            _navMeshAgent.Stop();
        }

        /// <summary>
        /// Receive the specified amount of damage.
        /// </summary>
        /// <param name="dmg">The DMG.</param>
        public void ReceiveDamage(float dmg)
        {
            _currentHealth -= dmg;
            if (_currentHealth <= 0f)
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

        /// <summary>
        /// Makes this unit attack any enemies within range.
        /// </summary>
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