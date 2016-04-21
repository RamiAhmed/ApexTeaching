namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    /// <summary>
    /// Scorer for evaluating whether this unit has a resource component in memory
    /// </summary>
    /// <seealso cref="Apex.AI.ContextualScorerBase" />
    public sealed class HasResourceInMemory : ContextualScorerBase
    {
        [ApexSerialization, FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            var unit = c.unit;

            var observations = unit.observations;
            var count = observations.Count;
            if (count == 0)
            {
                // unit has no observations
                return 0f;
            }

            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var resource = obs.GetComponent<ResourceComponent>();
                if (resource == null)
                {
                    // observation is not a resource
                    continue;
                }

                // at least one observed object is a resource component
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}