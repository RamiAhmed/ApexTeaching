namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Context class for all AI units
    /// </summary>
    /// <seealso cref="Apex.AI.IAIContext" />
    public class AIContext : IAIContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AIContext"/> class.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        public AIContext(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.unit = gameObject.GetComponent<UnitBase>();
            this.sampledCells = new List<Cell>(64);
        }

        /// <summary>
        /// Gets the game object.
        /// </summary>
        /// <value>
        /// The game object.
        /// </value>
        public GameObject gameObject
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public UnitBase unit
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sampled cells.
        /// </summary>
        /// <value>
        /// The sampled cells.
        /// </value>
        public List<Cell> sampledCells
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 position
        {
            get { return this.gameObject.transform.position; }
        }

        /// <summary>
        /// Gets or sets the resource target.
        /// </summary>
        /// <value>
        /// The resource target.
        /// </value>
        public ResourceComponent resourceTarget
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the attack target.
        /// </summary>
        /// <value>
        /// The attack target.
        /// </value>
        public ICanDie attackTarget
        {
            get;
            set;
        }
    }
}