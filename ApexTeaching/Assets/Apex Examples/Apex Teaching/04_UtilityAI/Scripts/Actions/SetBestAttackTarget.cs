namespace Apex.AI.Teaching
{
    using UnityEngine;
    using Utilities;

    public sealed class SetBestAttackTarget : ActionWithOptions<ICanDie>
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

            var list = ListBufferPool.GetBuffer<ICanDie>(Mathf.RoundToInt(count * 0.5f));
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var resource = obs.GetComponent<ICanDie>();
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

            c.attackTarget = best;
            ListBufferPool.ReturnBuffer(list);
        }
    }
}