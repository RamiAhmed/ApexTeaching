namespace Apex.AI.Teaching
{
    public sealed class MoveToOwnNest : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var unit = c.unit;
            unit.MoveTo(unit.nest.transform.position);
        }
    }
}