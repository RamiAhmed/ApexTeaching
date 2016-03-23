namespace Apex.AI.Teaching
{
    public sealed class MoveToBestCell : ActionWithOptions<Cell>
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var best = this.GetBest(c, c.sampledCells);
            if (best == null)
            {
                return;
            }

            c.unit.MoveTo(best.position);
        }
    }
}