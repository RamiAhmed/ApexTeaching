namespace Apex.AI.Teaching
{
    using UnityEngine;
    using Utilities;

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
                return;
            }

            var list = ListBufferPool.GetBuffer<ResourceComponent>(Mathf.RoundToInt(count * 0.5f));
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var resource = obs.GetComponent<ResourceComponent>();
                if (resource == null)
                {
                    continue;
                }

                list.Add(resource);
            }

            var best = this.GetBest(c, list);
            if (best == null)
            {
                ListBufferPool.ReturnBuffer(list);
                return;
            }

            c.resourceTarget = best;
            ListBufferPool.ReturnBuffer(list);
        }
    }
}