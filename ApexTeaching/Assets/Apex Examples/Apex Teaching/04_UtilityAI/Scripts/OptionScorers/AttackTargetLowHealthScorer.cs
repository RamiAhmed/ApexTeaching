namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    public sealed class AttackTargetLowHealthScorer : OptionScorerBase<ICanDie>
    {
        [ApexSerialization]
        public float maxScore = 10f;

        [ApexSerialization]
        public float factor = 1f;

        public override float Score(IAIContext context, ICanDie option)
        {
            var health = option.currentHealth * this.factor;
            return Mathf.Max(0f, this.maxScore - health);
        }
    }
}