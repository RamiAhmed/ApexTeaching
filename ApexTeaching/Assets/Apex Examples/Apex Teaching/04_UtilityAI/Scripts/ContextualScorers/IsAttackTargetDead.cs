namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class IsAttackTargetDead : ContextualScorerBase
    {
        [ApexSerialization]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            if (c.attackTarget == null)
            {
                return 0f;
            }

            if (c.attackTarget.isDead)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}