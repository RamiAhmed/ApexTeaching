namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for making a unit wander to a randomly generated location.
    /// </summary>
    /// <seealso cref="Apex.AI.ActionBase" />
    public sealed class RandomWander : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((AIContext)context).unit.RandomWander();
        }
    }
}