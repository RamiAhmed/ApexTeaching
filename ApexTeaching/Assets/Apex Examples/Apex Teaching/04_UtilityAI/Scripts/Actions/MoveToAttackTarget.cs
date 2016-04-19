namespace Apex.AI.Teaching
{
    /// <summary>
    /// Action class for making a unit move to its attack target (if valid)
    /// </summary>
    /// <seealso cref="Apex.AI.ActionBase" />
    public sealed class MoveToAttackTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var attackTarget = c.attackTarget;
            if (attackTarget == null)
            {
                // Attack target has not been set, thus unit cannot move to attack target
                return;
            }

            var nest = attackTarget.gameObject.GetComponent<NestStructure>();
            if (nest != null)
            {
                c.unit.MoveTo(nest.GetReturningPosition(c.unit));
            }
            else
            {
                c.unit.MoveTo(attackTarget.transform.position);
            }
        }
    }
}