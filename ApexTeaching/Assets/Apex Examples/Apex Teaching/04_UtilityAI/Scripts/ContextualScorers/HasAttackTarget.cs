namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class HasAttackTarget : ContextualScorerBase
    {
        [ApexSerialization]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            if (c.attackTarget != null)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}