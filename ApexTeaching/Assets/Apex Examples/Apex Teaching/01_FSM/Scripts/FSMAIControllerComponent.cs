namespace Apex.AI.Teaching
{
    public sealed class FSMAIControllerComponent : AIComponentBase<AIController>
    {
        public int desiredHarvesterCount = 10;

        protected override void ExecuteAI()
        {
            var resources = _entity.currentResources;
            if (resources <= 0)
            {
                // AI has no resources, cannot do anything
                return;
            }

            var harvesters = 0;
            var fighters = 0;
            var exploders = 0;

            var count = _entity.units.Count;
            for (int i = 0; i < count; i++)
            {
                var unit = _entity.units[i];
                if (unit.type == UnitType.Harvester)
                {
                    harvesters++;
                }
                else if (unit.type == UnitType.Warrior)
                {
                    fighters++;
                }
                else if (unit.type == UnitType.Blaster)
                {
                    exploders++;
                }
            }

            var nest = _entity.nest;
            if (harvesters < this.desiredHarvesterCount)
            {
                nest.BuildUnit(UnitType.Harvester);
                return;
            }

            if (fighters <= exploders)
            {
                nest.BuildUnit(UnitType.Warrior);
            }
            else
            {
                nest.BuildUnit(UnitType.Blaster);
            }
        }
    }
}