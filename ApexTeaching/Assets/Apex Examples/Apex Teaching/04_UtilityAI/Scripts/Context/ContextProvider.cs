namespace Apex.AI.Teaching
{
    using System;
    using Components;
    using UnityEngine;

    public sealed class ContextProvider : MonoBehaviour, IContextProvider
    {
        private AIContext _context;

        private void OnEnable()
        {
            _context = new AIContext(this.gameObject);
        }

        public IAIContext GetContext(Guid aiId)
        {
            return _context;
        }
    }
}