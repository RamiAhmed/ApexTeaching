namespace Apex.AI.Teaching
{
    public sealed class Attack : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((AIContext)context).unit.Attack();
        }
    }
}