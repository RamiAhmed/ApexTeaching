namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    public sealed class CellAvgProximityToEnemiesScorer : OptionScorerBase<Cell>
    {
        [ApexSerialization]
        public float maxScore = 100f;

        [ApexSerialization]
        public float distanceFactor = 0.1f;

        public override float Score(IAIContext context, Cell option)
        {
            var c = (AIContext)context;
            var observations = c.unit.observations;
            var count = observations.Count;
            if (count == 0)
            {
                return 0f;
            }

            var enemiesCount = 0;
            var distanceSummed = 0f;
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var unit = obs.GetComponent<UnitBase>();
                if (unit == null || c.unit.IsAllied(unit))
                {
                    continue;
                }

                distanceSummed += (unit.transform.position - option.position).magnitude * this.distanceFactor;
                enemiesCount++;
            }

            distanceSummed /= enemiesCount;
            return Mathf.Max(0f, this.maxScore - distanceSummed);
        }
    }
}