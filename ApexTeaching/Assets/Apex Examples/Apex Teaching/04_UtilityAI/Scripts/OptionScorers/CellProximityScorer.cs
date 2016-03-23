namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    /// <summary>
    /// OptionScorer for scoring cells depending on their proximity to this unit
    /// </summary>
    /// <seealso cref="Apex.AI.OptionScorerBase{Apex.AI.Teaching.Cell}" />
    public sealed class CellProximityScorer : OptionScorerBase<Cell>
    {
        [ApexSerialization, FriendlyName("Max Score", "The highest score that this scorer can output to an option")]
        public float maxScore = 10f;

        [ApexSerialization, FriendlyName("Distance Factor", "A factor used to multiply the calculated distance by")]
        public float distanceFactor = 0.1f;

        public override float Score(IAIContext context, Cell option)
        {
            var c = (AIContext)context;
            var distance = (c.position - option.position).magnitude * this.distanceFactor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}