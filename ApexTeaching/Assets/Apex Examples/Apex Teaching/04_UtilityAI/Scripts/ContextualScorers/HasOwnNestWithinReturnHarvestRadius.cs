namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    /// <summary>
    /// Scorer for evaluating whether this unit's nest is within its return harvest radius.
    /// </summary>
    /// <seealso cref="Apex.AI.ContextualScorerBase" />
    public sealed class HasOwnNestWithinReturnHarvestRadius : ContextualScorerBase
    {
        [ApexSerialization, FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            var unit = c.unit as HarvesterUnit;
            if (unit == null)
            {
                // unit is not a harvester unit, so stop here
                return 0f;
            }

            var distanceSqr = (unit.nest.transform.position - c.position).sqrMagnitude;
            if (distanceSqr > (unit.returnHarvestRadius * unit.returnHarvestRadius))
            {
                // distance to the nest is more than the unit's return harvest radius
                return this.not ? this.score : 0f;
            }

            return this.not ? 0f : this.score;
        }
    }
}