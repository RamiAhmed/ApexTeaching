namespace Apex.AI.Teaching
{
    using UnityEngine;

    public class ControllerContext : IAIContext
    {
        public ControllerContext(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.controller = gameObject.GetComponent<AIController>();
        }

        public GameObject gameObject
        {
            get;
            private set;
        }

        public AIController controller
        {
            get;
            private set;
        }
    }
}