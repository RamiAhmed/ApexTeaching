namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class CanCarryMoreHarvest : ContextualScorerBase
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

            if (unit.currentCarriedResources < unit.maxCarriableResources)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}