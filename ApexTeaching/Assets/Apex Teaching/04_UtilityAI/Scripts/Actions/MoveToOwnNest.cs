namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for making a unit move to its nest.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionBase" />
    public sealed class MoveToOwnNest : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            c.unit.MoveTo(c.unit.nest.GetReturningPosition(c.unit));
        }
    }
}