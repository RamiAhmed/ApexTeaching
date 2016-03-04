namespace Apex.AI.Teaching
{
    using UnityEngine;

    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _nestGO;

        [SerializeField]
        private Color _color;

        private NestStructure _nest;

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