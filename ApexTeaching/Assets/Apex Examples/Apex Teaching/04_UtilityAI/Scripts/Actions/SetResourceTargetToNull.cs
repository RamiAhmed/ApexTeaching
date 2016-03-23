namespace Apex.AI.Teaching
{
    public sealed class SetResourceTargetToNull : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((AIContext)context).resourceTarget = null;
        }
    }
}