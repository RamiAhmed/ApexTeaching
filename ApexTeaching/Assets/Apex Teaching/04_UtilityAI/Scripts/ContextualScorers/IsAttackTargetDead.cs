namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    /// <summary>
    /// Scorer for evaluating whether this unit's attack target is dead.
    /// </summary>
    /// <seealso cref="Apex.AI.ContextualScorerBase" />
    public sealed class IsAttackTargetDead : ContextualScorerBase
    {
        [ApexSerialization, FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            if (c.attackTarget == null)
            {
                // unit has no attack target
                return 0f;
            }

            if (c.attackTarget.isDead)
            {
                // the attack target is dead
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}