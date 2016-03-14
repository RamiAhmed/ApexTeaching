namespace Apex.AI.Teaching
{
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class Path : List<Vector3>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Path"/> class.
        /// </summary>
        /// <param name="preallocation">The preallocated capacity for the path.</param>
        public Path(int preallocation)
            : base(preallocation)
        {
        }

        /// <summary>
        /// Pops the first element in the list, removing it from the list and returning it to the caller.
        /// </summary>
        /// <returns>The first Vector3 element in the list, or default(Vector3) if none are in the list.</returns>
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