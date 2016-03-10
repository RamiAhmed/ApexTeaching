namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Path : List<Vector3>
    {
        public Path(IEnumerable<Vector3> collection)
            : base(collection)
        {
        }

        public Path(int preallocation)
            : base(preallocation)
        {
        }

        public Path()
            : base()
        {
        }

        public Vector3 last
        {
            get { return this.Count > 0 ? this[this.Count - 1] : default(Vector3); }
        }

        public Vector3 Pop()
        {
            if (this.Count == 0)
            {
                return default(Vector3);
            }

            var item = this[0];
            this.RemoveAt(0);
            return item;
        }
    }
}