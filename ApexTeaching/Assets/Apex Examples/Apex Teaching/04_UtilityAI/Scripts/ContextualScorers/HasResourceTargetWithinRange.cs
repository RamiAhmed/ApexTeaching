namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class HasResourceTargetWithinRange : ContextualScorerBase
    {
        [ApexSerialization, MemberDependency("useAttackRadius", false)]
        public float range = 10f;

        [ApexSerialization]
        public bool not;

        [ApexSerialization]
        public bool useAttackRadius;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            var resourceTarget = c.resourceTarget;

            var distance = (c.position - resourceTarget.transform.position).sqrMagnitude;
            var rangeSqr = this.useAttackRadius ? c.unit.attackRadius * c.unit.attackRadius : this.range * this.range;
            if (distance < rangeSqr)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}