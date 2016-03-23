namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class HasEnemyUnitInMemory : ContextualScorerBase
    {
        [ApexSerialization, MemberDependency("useAttackRadius", false), MemberDependency("useScanRadius", false)]
        public float range = 10f;

        [ApexSerialization, MemberDependency("useAttackRadius", false)]
        public bool useScanRadius;

        [ApexSerialization, MemberDependency("useScanRadius", false)]
        public bool useAttackRadius;

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
                var obsUnit = obs.GetComponent<UnitBase>();
                if (obsUnit == null)
                {
                    continue;
                }

                if (rangeSqr > 0f)
                {
                    var distance = (obsUnit.transform.position - c.position).sqrMagnitude;
                    if (distance > rangeSqr)
                    {
                        continue;
                    }
                }

                if (unit.IsAllied(obsUnit))
                {
                    continue;
                }

                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}