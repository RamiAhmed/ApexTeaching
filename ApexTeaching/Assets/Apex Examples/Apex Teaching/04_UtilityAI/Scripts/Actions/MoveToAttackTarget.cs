namespace Apex.AI.Teaching
{
    public sealed class MoveToAttackTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var attackTarget = c.attackTarget;
            if (attackTarget == null)
            {
                return;
            }

            c.unit.MoveTo(attackTarget.transform.position);
        }
    }
}