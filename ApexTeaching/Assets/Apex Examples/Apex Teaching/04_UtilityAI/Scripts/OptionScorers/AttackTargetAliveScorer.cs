namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    /// <summary>
    /// OptionScorer for scoring ICanDie (candidate attack targets) depending on whether they are dead or alive
    /// </summary>
    /// <seealso cref="Apex.AI.OptionScorerBase{Apex.AI.Teaching.ICanDie}" />
    public sealed class AttackTargetAliveScorer : OptionScorerBase<ICanDie>
    {
        [ApexSerialization, FriendlyName("Score", "The score to give to ICanDies that are alive")]
        public float score = 10f;

        [ApexSerialization, FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context, ICanDie option)
        {
            if (!option.isDead)
            {
                // The option being evaluated is not dead
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}