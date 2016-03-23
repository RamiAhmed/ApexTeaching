namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for making a unit return its collected harvest to its nest.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionBase" />
    public sealed class ReturnHarvest : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var unit = c.unit as HarvesterUnit;
            if (unit == null)
            {
                // Only harvester units may return harvest
                return;
            }

            var distanceSqr = (unit.nest.transform.position - c.position).sqrMagnitude;
            if (distanceSqr > (unit.returnHarvestRadius * unit.returnHarvestRadius))
            {
                // the nest is too far away and so this unit cannot return harvest
                return;
            }

            unit.ReturnHarvest();
        }
    }
}