namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public class AIContext : IAIContext
    {
        public AIContext(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.unit = gameObject.GetComponent<UnitBase>();
            this.steerForPath = gameObject.GetComponent<SteerForPath>();
            this.sampledCells = new List<Cell>(64);
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

        public List<Cell> sampledCells
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