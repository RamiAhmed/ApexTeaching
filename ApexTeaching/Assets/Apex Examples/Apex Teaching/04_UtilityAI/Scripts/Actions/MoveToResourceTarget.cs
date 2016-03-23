namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for making a unit move to its resource target if valid.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionBase" />
    public sealed class MoveToResourceTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            if (c.resourceTarget == null)
            {
                // resource target has not been set, no move possible
                return;
            }

            c.unit.MoveTo(c.resourceTarget.transform.position);
        }
    }
}