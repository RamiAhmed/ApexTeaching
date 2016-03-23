namespace Apex.AI.Teaching
{
    using System;
    using Components;
    using UnityEngine;

    /// <summary>
    /// ContextProvider class for all AI units.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="Apex.AI.Components.IContextProvider" />
    public sealed class ContextProvider : MonoBehaviour, IContextProvider
    {
        private AIContext _context;

        private void OnEnable()
        {
            // Build a context object for this unit from its GameObject.
            _context = new AIContext(this.gameObject);
        }

        /// <summary>
        /// Retrieves the context instance.
        /// </summary>
        /// <param name="aiId">The Id of the requesting AI.</param>
        /// <returns>
        /// The concrete context instance for the requesting AI client.
        /// </returns>
        public IAIContext GetContext(Guid aiId)
        {
            return _context;
        }
    }
}