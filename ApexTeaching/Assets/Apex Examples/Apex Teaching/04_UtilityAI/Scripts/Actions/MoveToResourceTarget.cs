namespace Apex.AI.Teaching
{
    public sealed class MoveToResourceTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            if (c.resourceTarget == null)
            {
                return;
            }

            c.unit.MoveTo(c.resourceTarget.transform.position);
        }
    }
}