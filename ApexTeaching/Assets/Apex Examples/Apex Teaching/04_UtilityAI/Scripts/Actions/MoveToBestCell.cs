namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for making a unit move to the highest scoring cell.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionWithOptions{Apex.AI.Teaching.Cell}" />
    public sealed class MoveToBestCell : ActionWithOptions<Cell>
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var best = this.GetBest(c, c.sampledCells);
            if (best == null)
            {
                // Best (Highest-scoring) cell is null, so no move is possible
                return;
            }

            c.unit.MoveTo(best.position);
        }
    }
}