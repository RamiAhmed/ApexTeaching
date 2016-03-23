namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    public sealed class AttackTargetProximityScorer : OptionScorerBase<ICanDie>
    {
        [ApexSerialization]
        public float maxScore = 10f;

        [ApexSerialization]
        public float distanceFactor = 0.1f;

        public override float Score(IAIContext context, ICanDie option)
        {
            var c = (AIContext)context;
            var distance = (c.position - option.transform.position).magnitude * this.distanceFactor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}