namespace Apex.AI.Teaching
{
    public sealed class RandomWander : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            c.unit.RandomWander();
        }
    }
}