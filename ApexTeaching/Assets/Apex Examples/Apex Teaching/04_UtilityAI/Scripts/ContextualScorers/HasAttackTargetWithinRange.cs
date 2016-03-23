namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class HasAttackTargetWithinRange : ContextualScorerBase
    {
        [ApexSerialization, MemberDependency("useScanRadius", false), MemberDependency("useAttackRadius", false)]
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
            var attackTarget = c.attackTarget;
            if (attackTarget == null)
            {
                return 0f;
            }

            var unit = c.unit;
            var rangeSqr = this.range * this.range;
            if (this.useScanRadius)
            {
                rangeSqr = unit.scanRadius * unit.scanRadius;
            }
            else if (this.useAttackRadius)
            {
                rangeSqr = unit.attackRadius * unit.attackRadius;
            }

            var distance = (c.position - attackTarget.transform.position).sqrMagnitude;
            if (distance < rangeSqr)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}