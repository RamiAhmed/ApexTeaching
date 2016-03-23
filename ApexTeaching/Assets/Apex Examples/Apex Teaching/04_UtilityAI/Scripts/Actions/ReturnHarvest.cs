namespace Apex.AI.Teaching
{
    public sealed class ReturnHarvest : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var unit = c.unit as HarvesterUnit;
            if (unit == null)
            {
                return;
            }

            var distance = (unit.nest.transform.position - c.position).sqrMagnitude;
            if (distance > (unit.returnHarvestRadius * unit.returnHarvestRadius))
            {
                return;
            }

            unit.ReturnHarvest();
        }
    }
}