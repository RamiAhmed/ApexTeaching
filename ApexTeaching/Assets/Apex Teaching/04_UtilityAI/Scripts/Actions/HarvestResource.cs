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
                // unit must be a harvester unit to execute this action
                return;
            }

            if (c.resourceTarget == null || c.resourceTarget.currentResources <= 0)
            {
                // Resource target is null or has no resources, cannot harvest from it
                return;
            }

            unit.Harvest(c.resourceTarget);
        }
    }
}