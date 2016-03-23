namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class HasOwnNestWithinReturnHarvestRadius : ContextualScorerBase
    {
        [ApexSerialization]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            var unit = c.unit as HarvesterUnit;
            if (unit == null)
            {
                return 0f;
            }

            var distance = (unit.nest.transform.position - c.position).sqrMagnitude;
            if (distance > (unit.returnHarvestRadius * unit.returnHarvestRadius))
            {
                return this.not ? this.score : 0f;
            }

            return this.not ? 0f : this.score;
        }
    }
}