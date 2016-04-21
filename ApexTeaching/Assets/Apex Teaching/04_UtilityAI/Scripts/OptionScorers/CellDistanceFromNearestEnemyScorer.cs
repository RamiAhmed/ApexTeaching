namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    /// <summary>
    /// OptionScorer for scoring cells that are farther away from the nearest enemy unit
    /// </summary>
    /// <seealso cref="Apex.AI.OptionScorerBase{Apex.AI.Teaching.Cell}" />
    public sealed class CellDistanceFromNearestEnemyScorer : OptionScorerBase<Cell>
    {
        [ApexSerialization, FriendlyName("Max Score", "The highest score that this scorer can output to an option")]
        public float maxScore = 10f;

        [ApexSerialization, FriendlyName("Distance Factor", "A factor used to multiply the calculated distance by")]
        public float distanceFactor = 0.1f;

        public override float Score(IAIContext context, Cell option)
        {
            var c = (AIContext)context;
            var observations = c.unit.observations;
            var count = observations.Count;
            if (count == 0)
            {
                // unit has no observations
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
                // there is no nearest enemy unit
                return 0f;
            }

            // return the distance multiplied by the factor, but never let it surpass the max score
            var distance = (nearest.transform.position - option.position).magnitude * this.distanceFactor;
            return Mathf.Min(distance, this.maxScore);
        }
    }
}