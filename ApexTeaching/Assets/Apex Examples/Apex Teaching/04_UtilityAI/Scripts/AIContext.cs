namespace Apex.AI.Teaching
{
    using UnityEngine;

    public class AIContext : IAIContext
    {
        public AIContext(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.unit = gameObject.GetComponent<UnitBase>();
            this.steerForPath = gameObject.GetComponent<SteerForPath>();
        }

        public GameObject gameObject
        {
            get;
            private set;
        }

        public SteerForPath steerForPath
        {
            get;
            private set;
        }

        public UnitBase unit
        {
            get;
            private set;
        }

        public Vector3 position
        {
            get { return this.gameObject.transform.position; }
        }

        public ResourceComponent resourceTarget
        {
            get;
            set;
        }

        public ICanDie attackTarget
        {
            get;
            set;
        }
    }
}