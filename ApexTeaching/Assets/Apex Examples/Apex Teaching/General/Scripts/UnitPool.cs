namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Pools or caches units so that they can be reused.
    /// </summary>
    public sealed class UnitPool
    {
        private static int _nextId;
        private Queue<UnitBase> _pool;
        private GameObject _prefab;
        private GameObject _host;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitPool"/> class.
        /// </summary>
        /// <param name="prefab">The prefab from which to create the unit.</param>
        /// <param name="host">The host that will be the parent of unit instances.</param>
        /// <param name="initialInstanceCount">The initial instance count.</param>
        public UnitPool(GameObject prefab, GameObject host, int initialInstanceCount)
        {
            _pool = new Queue<UnitBase>(initialInstanceCount);
            _prefab = prefab;
            _host = host;

            //Instantiate and queue up the initial number of entities
            for (int i = 0; i < initialInstanceCount; i++)
            {
                _pool.Enqueue(CreateInstance());
            }
        }

        public int count
        {
            get { return _pool.Count; }
        }

        /// <summary>
        /// Gets an unit from the pool and places it at the specified position and rotation.
        /// If the pool is empty a new instance is created.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns>The entity</returns>
        public UnitBase Get(Vector3 position, Quaternion rotation)
        {
            UnitBase unit;
            if (_pool.Count > 0)
            {
                unit = _pool.Dequeue();
            }
            else
            {
                unit = CreateInstance();
            }

            var t = unit.gameObject.transform;

            t.position = position;
            t.rotation = rotation;

            unit.id = _nextId++;
            unit.gameObject.SetActive(true);
            return unit;
        }

        /// <summary>
        /// Returns the specified unit to the pool.
        /// </summary>
        /// <param name="item">The entity.</param>
        public void Return(UnitBase item)
        {
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }

        private UnitBase CreateInstance()
        {
            var go = (GameObject)GameObject.Instantiate(_prefab);
            if (_host != null)
            {
                go.transform.SetParent(_host.transform);
            }

            go.SetActive(false);
            return (UnitBase)go.GetComponent(typeof(UnitBase));
        }
    }
}