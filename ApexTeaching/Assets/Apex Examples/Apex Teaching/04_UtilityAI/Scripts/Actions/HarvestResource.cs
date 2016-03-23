namespace Apex.AI.Teaching
{
    public sealed class HarvestResource : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var unit = c.unit as HarvesterUnit;
            if (unit == null)
            {
                return;
            }

            if (c.resourceTarget == null || c.resourceTarget.currentResources <= 0)
            {
                return;
            }

            unit.Harvest(c.resourceTarget);
        }
    }
}