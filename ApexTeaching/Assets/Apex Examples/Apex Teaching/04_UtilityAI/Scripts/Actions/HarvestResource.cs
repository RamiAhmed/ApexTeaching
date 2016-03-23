namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for harvesting resources from the current resource target.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionBase" />
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