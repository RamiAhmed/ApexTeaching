namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class AttackTargetAliveScorer : OptionScorerBase<ICanDie>
    {
        [ApexSerialization]
        public float score = 10f;

        [ApexSerialization]
        public bool not;

        public override float Score(IAIContext context, ICanDie option)
        {
            if (!option.isDead)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}