namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for making a unit attack.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionBase" />
    public sealed class Attack : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((AIContext)context).unit.Attack();
        }
    }
}