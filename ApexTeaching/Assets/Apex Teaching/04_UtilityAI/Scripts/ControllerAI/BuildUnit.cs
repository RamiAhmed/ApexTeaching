namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class BuildUnit : ActionBase
    {
        [ApexSerialization]
        public UnitType unitType;

        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;
            c.controller.nest.SpawnUnit(this.unitType);
        }
    }
}