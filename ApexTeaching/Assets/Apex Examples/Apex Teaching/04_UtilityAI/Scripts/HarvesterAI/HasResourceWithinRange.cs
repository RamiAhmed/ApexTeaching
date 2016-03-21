namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class HasResourceWithinRange : ContextualScorerBase
    {
        [ApexSerialization]
        public float range = 10f;

        [ApexSerialization]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            var unit = c.unit;

            var observations = unit.observations;
            var count = observations.Count;
            if (count == 0)
            {
                return 0f;
            }

            var rangeSqr = this.range * this.range;
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var resource = obs.GetComponent<ResourceComponent>();
                if (resource == null || resource.currentResources <= 0)
                {
                    continue;
                }

                if ((resource.transform.position - c.position).sqrMagnitude > rangeSqr)
                {
                    continue;
                }

                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}