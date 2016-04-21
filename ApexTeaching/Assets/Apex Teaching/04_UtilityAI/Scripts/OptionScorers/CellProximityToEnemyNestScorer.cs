namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    public sealed class CellProximityToEnemyNestScorer : OptionScorerBase<Cell>
    {
        [ApexSerialization]
        public float maxScore;

        [ApexSerialization]
        public float distanceFactor = 0.1f;

        public override float Score(IAIContext context, Cell option)
        {
            var c = (AIContext)context;
            var enemyNest = c.unit.nest.enemyNest;
            var distance = (enemyNest.transform.position - option.position).magnitude * this.distanceFactor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}