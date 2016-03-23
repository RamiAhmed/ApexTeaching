namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for nulling the resource target.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionBase" />
    public sealed class SetResourceTargetToNull : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((AIContext)context).resourceTarget = null;
        }
    }
}