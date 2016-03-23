namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    /// <summary>
    /// OptionScorer for scoring ICanDies with low health
    /// </summary>
    /// <seealso cref="Apex.AI.OptionScorerBase{Apex.AI.Teaching.ICanDie}" />
    public sealed class AttackTargetLowHealthScorer : OptionScorerBase<ICanDie>
    {
        [ApexSerialization, FriendlyName("Max Score", "The highest score that this scorer can output to an option")]
        public float maxScore = 10f;

        [ApexSerialization, FriendlyName("Factor", "A factor used to multiply the option's current health by")]
        public float factor = 1f;

        public override float Score(IAIContext context, ICanDie option)
        {
            // low health options score closer to MaxScore than options with more health
            var health = option.currentHealth * this.factor;
            return Mathf.Max(0f, this.maxScore - health);
        }
    }
}