namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for scanning unblocked grid cells and storing them in the context.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionBase" />
    public sealed class ScanForGridCells : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            Grid.instance.GetUnblockedCells(c.position, c.unit.scanRadius, c.sampledCells);
        }
    }
}