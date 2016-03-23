namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    /// <summary>
    /// An OptionScorer for scoring cells based on their average proximity to all observed enemy units.
    /// </summary>
    /// <seealso cref="Apex.AI.OptionScorerBase{Apex.AI.Teaching.Cell}" />
    public sealed class CellAvgProximityToEnemiesScorer : OptionScorerBase<Cell>
    {
        [ApexSerialization, FriendlyName("Max Score", "The highest score that this scorer can output to an option")]
        public float maxScore = 100f;

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

            var enemiesCount = 0;
            var distanceSummed = 0f;
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var unit = obs.GetComponent<UnitBase>();
                if (unit == null || c.unit.IsAllied(unit))
                {
                    // observation is not a unit or is an allied unit
                    continue;
                }

                // add the distance to the summed distance and increment the count
                distanceSummed += (unit.transform.position - option.position).magnitude * this.distanceFactor;
                enemiesCount++;
            }

            // divide the summed distance by the count to get it averaged
            distanceSummed /= enemiesCount;
            return Mathf.Max(0f, this.maxScore - distanceSummed);
        }
    }
}