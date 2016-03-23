namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class IsMoving : ContextualScorerBase
    {
        [ApexSerialization]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            if (c.unit.isMoving)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}