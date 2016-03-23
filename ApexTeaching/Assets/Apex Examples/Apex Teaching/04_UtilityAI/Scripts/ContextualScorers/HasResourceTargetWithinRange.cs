namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    /// <summary>
    /// Scorer for evaluating whether this unit has a resource target within a given range.
    /// </summary>
    /// <seealso cref="Apex.AI.ContextualScorerBase" />
    public sealed class HasResourceTargetWithinRange : ContextualScorerBase
    {
        [ApexSerialization, MemberDependency("useAttackRadius", false), FriendlyName("A custom range to use to evaluate whether the resource target is within")]
        public float range = 10f;

        [ApexSerialization, FriendlyName("Use Attack Radius", "Set to true to use the unit's attackRadius as the range")]
        public bool useAttackRadius;

        [ApexSerialization, FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            if (c.resourceTarget == null)
            {
                // unit has no resource target
                return 0f;
            }

            var distanceSqr = (c.position - c.resourceTarget.transform.position).sqrMagnitude;
            var range = this.useAttackRadius ? c.unit.attackRadius : this.range;
            if (distanceSqr < (range * range))
            {
                // the resource target is within the desired range
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}