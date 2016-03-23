namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    public sealed class NearestResourceScorer : OptionScorerBase<ResourceComponent>
    {
        [ApexSerialization]
        public float maxScore = 100f;

        [ApexSerialization]
        public float distanceFactor = 0.1f;

        public override float Score(IAIContext context, ResourceComponent option)
        {
            var c = (AIContext)context;
            var distance = (c.position - option.transform.position).magnitude * this.distanceFactor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}