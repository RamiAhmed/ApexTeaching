namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    public sealed class CellDistanceFromNearestEnemyScorer : OptionScorerBase<Cell>
    {
        [ApexSerialization]
        public float maxScore = 10f;

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

            GameObject nearest = null;
            var shortest = float.MaxValue;
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var unit = obs.GetComponent<UnitBase>();
                if (unit == null || c.unit.IsAllied(unit))
                {
                    // ignore non-units and allied units
                    continue;
                }

                var distanceSqr = (option.position - unit.transform.position).sqrMagnitude;
                if (distanceSqr < shortest)
                {
                    distanceSqr = shortest;
                    nearest = obs;
                }
            }

            if (nearest == null)
            {
                return 0f;
            }

            var distance = (nearest.transform.position - option.position).magnitude * this.distanceFactor;
            return Mathf.Min(distance, this.maxScore);
        }
    }
}