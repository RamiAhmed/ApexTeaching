namespace Apex.AI.Teaching
{
    public sealed class ScanForGridCells : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            Grid.instance.GetCells(c.position, c.unit.scanRadius, c.sampledCells);
        }
    }
}