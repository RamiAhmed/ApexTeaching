namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class HasEnemyNestInMemory : ContextualScorerBase
    {
        [ApexSerialization, MemberDependency("useAttackRadius", false), MemberDependency("useScanRadius", false), FriendlyName("Range", "The range to evaluate whether an enemy unit is within. Set to 0 to disable range check")]
        public float range = 10f;

        [ApexSerialization, MemberDependency("useAttackRadius", false), FriendlyName("Use Scan Radius", "Set to true to use the unit's scanRadius as the range")]
        public bool useScanRadius;

        [ApexSerialization, MemberDependency("useScanRadius", false), FriendlyName("Use Attack Radius", "Set to true to use the unit's attackRadius as the range")]
        public bool useAttackRadius;

        [ApexSerialization, FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            var unit = c.unit;
            var observations = c.unit.observations;
            var count = observations.Count;
            if (count == 0)
            {
                return 0f;
            }

            // get the right range and square it once (for performance)
            var rangeSqr = this.range * this.range;
            if (this.useScanRadius)
            {
                rangeSqr = unit.scanRadius * unit.scanRadius;
            }
            else if (this.useAttackRadius)
            {
                rangeSqr = unit.attackRadius * unit.attackRadius;
            }

            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var obsNest = obs.GetComponent<NestStructure>();
                if (obsNest == null)
                {
                    continue;
                }

                if (unit.IsAllied(obsNest))
                {
                    continue;
                }

                if (rangeSqr > 0f)
                {
                    // only evaluate range if relevant
                    var distanceSqr = (obsNest.transform.position - c.position).sqrMagnitude;
                    if (distanceSqr > rangeSqr)
                    {
                        // unit is farther away than the desired range
                        continue;
                    }
                }

                // enemy nest is within range
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}