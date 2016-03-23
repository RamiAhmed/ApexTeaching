namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    public sealed class CellProximityToAttackTargetScorer : OptionScorerBase<Cell>
    {
        [ApexSerialization]
        public float maxScore = 10f;

        [ApexSerialization]
        public float distanceFactor = 0.1f;

        public override float Score(IAIContext context, Cell option)
        {
            var c = (AIContext)context;
            var distance = (c.attackTarget.transform.position - option.position).magnitude * this.distanceFactor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}