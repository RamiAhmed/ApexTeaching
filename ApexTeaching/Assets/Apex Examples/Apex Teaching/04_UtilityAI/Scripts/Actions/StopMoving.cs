namespace Apex.AI.Teaching
{
    public sealed class StopMoving : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((AIContext)context).unit.StopMoving();
        }
    }
}