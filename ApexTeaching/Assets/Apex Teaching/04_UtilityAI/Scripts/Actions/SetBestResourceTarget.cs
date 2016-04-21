namespace Apex.AI.Teaching
{
    using UnityEngine;
    using Utilities;

    /// <summary>
    /// Action class for setting the highest scoring observed resource component as the unit's current resource target
    /// </summary>
    /// <seealso cref="Apex.AI.ActionWithOptions{Apex.AI.Teaching.ResourceComponent}" />
    public sealed class SetBestResourceTarget : ActionWithOptions<ResourceComponent>
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var unit = c.unit;

            var observations = unit.observations;
            var count = observations.Count;
            if (count == 0)
            {
                // unit has no observations
                return;
            }

            // get a list from buffer pool
            var list = ListBufferPool.GetBuffer<ResourceComponent>(Mathf.RoundToInt(count * 0.25f));// Preallocation could probably be better, currently it is just a rough estimate that a quarther of the unit's observations could be resources
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var resource = obs.GetComponent<ResourceComponent>();
                if (resource == null)
                {
                    // observation is not a resource component
                    continue;
                }

                list.Add(resource);
            }

            var best = this.GetBest(c, list);
            if (best != null)
            {
                // highest scoring resource candidate i valid and so becomes the new resource target
                c.resourceTarget = best;
            }

            // Return the "borrowed" list from the buffer pool
            ListBufferPool.ReturnBuffer(list);
        }
    }
}