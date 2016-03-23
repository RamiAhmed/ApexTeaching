namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    public sealed class RichestResourceScorer : OptionScorerBase<ResourceComponent>
    {
        [ApexSerialization]
        public float maxScore = 100f;

        [ApexSerialization]
        public float factor = 1f;

        public override float Score(IAIContext context, ResourceComponent option)
        {
            var currentResources = option.currentResources * this.factor;
            return Mathf.Min(this.maxScore, currentResources);
        }
    }
}