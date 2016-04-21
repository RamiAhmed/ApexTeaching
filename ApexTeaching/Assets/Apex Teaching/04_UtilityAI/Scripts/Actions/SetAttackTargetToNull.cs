namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for nulling the attack target stored in the context.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionBase" />
    public sealed class SetAttackTargetToNull : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((AIContext)context).attackTarget = null;
        }
    }
}