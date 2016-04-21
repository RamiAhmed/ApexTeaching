namespace Apex.AI.Teaching
{
    using UnityEngine;
    using Utilities;

    /// <summary>
    /// Action class for setting the highest scoring ICanDie candidate to the current attack target.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionWithOptions{Apex.AI.Teaching.ICanDie}" />
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
                // unit has no observations of other things
                return;
            }

            // get a list to populate from the buffer pool
            var list = ListBufferPool.GetBuffer<ICanDie>(Mathf.RoundToInt(count * 0.5f)); // Preallocation could probably be better, currently it is just a rough estimate that half of the unit's observations could be units
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var canDie = obs.GetComponent<ICanDie>();
                if (canDie == null)
                {
                    // observation is not a "ICanDie", so ignore it
                    continue;
                }

                var obsUnit = obs.GetComponent<UnitBase>();
                if (obsUnit != null)
                {
                    if (unit.IsAllied(obsUnit))
                    {
                        // ignored allied units
                        continue;
                    }
                }

                var nest = obs.GetComponent<NestStructure>();
                if (nest != unit)
                {
                    if (unit.IsAllied(nest))
                    {
                        // ignore allied nests
                        continue;
                    }
                }

                list.Add(canDie);
            }

            var best = this.GetBest(c, list);
            if (best != null)
            {
                // the highest-scoring candidate is valid, so set it to be the new attack target
                c.attackTarget = best;
            }

            // return the list "borrowed" from the buffer pool
            ListBufferPool.ReturnBuffer(list);
        }
    }
}