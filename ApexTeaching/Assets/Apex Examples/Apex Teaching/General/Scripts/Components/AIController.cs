namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _nestGO;

        [SerializeField]
        private Color _color;

        [SerializeField]
        private int _currentResources;

        private NestStructure _nest;
        private List<UnitBase> _units;

        /// <summary>
        /// Gets a reference to the nest.
        /// </summary>
        /// <value>
        /// The nest.
        /// </value>
        public NestStructure nest
        {
            get { return _nest; }
        }

        /// <summary>
        /// Gets a list of all currently active units owned by this AI Controller
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        public List<UnitBase> units
        {
            get { return _units; }
        }

        /// <summary>
        /// Get the current amount of resources that this AI has - DO NOT MODIFY.
        /// </summary>
        /// <value>
        /// The current resources.
        /// </value>
        public int currentResources
        {
            get { return _currentResources; }
            set { _currentResources = value; }
        }

        /// <summary>
        /// Gets the AI's color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color color
        {
            get { return _color; }
        }

        private void Awake()
        {
            _units = new List<UnitBase>(30);

            // color nest
            var renderers = _nestGO.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].GetComponent<ParticleSystem>() == null)
                {
                    renderers[i].material.color = _color;
                }
            }

            // setup nest reference
            _nest = _nestGO.GetComponent<NestStructure>();
            _nest.controller = this;
        }
    }
}