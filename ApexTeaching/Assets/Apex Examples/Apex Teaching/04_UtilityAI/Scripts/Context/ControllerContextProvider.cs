namespace Apex.AI.Teaching
{
    using System;
    using Components;
    using UnityEngine;

    public sealed class ControllerContextProvider : MonoBehaviour, IContextProvider
    {
        private ControllerContext _context;

        private void OnEnable()
        {
            _context = new ControllerContext(this.gameObject);
        }

        private void OnDisable()
        {
            _context = null;
        }

        public IAIContext GetContext(Guid aiId)
        {
            return _context;
        }
    }
}