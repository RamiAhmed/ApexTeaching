namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    /// <summary>
    /// An OptionScorer for scoring ICanDies depending on their proximity to the unit
    /// </summary>
    /// <seealso cref="Apex.AI.OptionScorerBase{Apex.AI.Teaching.ICanDie}" />
    public sealed class AttackTargetProximityScorer : OptionScorerBase<ICanDie>
    {
        [ApexSerialization, FriendlyName("Max Score", "The highest score that this scorer can output to an option")]
        public float maxScore = 10f;

        [ApexSerialization, FriendlyName("Distance Factor", "A factor used to multiply the calculated distance by")]
        public float distanceFactor = 0.1f;

        public override float Score(IAIContext context, ICanDie option)
        {
            var c = (AIContext)context;
            var distance = (c.position - option.transform.position).magnitude * this.distanceFactor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}