namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for causing a unit to stop all movement instantly.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionBase" />
    public sealed class StopMoving : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((AIContext)context).unit.StopMoving();
        }
    }
}