namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class CanAffordUnitType : ContextualScorerBase
    {
        [ApexSerialization]
        public UnitType unitType;

        public override float Score(IAIContext context)
        {
            var c = (ControllerContext)context;
            var resources = c.controller.nest.currentResources;
            var cost = UnitCostManager.GetCost(this.unitType);
            if (resources >= cost)
            {
                return this.score;
            }

            return 0f;
        }
    }
}