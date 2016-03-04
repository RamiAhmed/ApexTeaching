namespace Apex.AI.Teaching
{
    public sealed class FSMAIControllerComponent : AIComponentBase<AIController>
    {
        public int desiredHarvesterCount = 10;

        protected override void ExecuteAI()
        {
            var nest = _entity.nest;
            var resources = nest.currentResources;
            if (resources <= 0)
            {
                // AI has no resources, cannot do anything
                return;
            }

            var harvesters = 0;
            var fighters = 0;
            var exploders = 0;

            var count = nest.units.Count;
            for (int i = 0; i < count; i++)
            {
                var unit = nest.units[i];
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

            if (harvesters < this.desiredHarvesterCount)
            {
                nest.SpawnUnit(UnitType.Harvester);
                return;
            }

            if (fighters <= exploders)
            {
                nest.SpawnUnit(UnitType.Warrior);
            }
            else
            {
                nest.SpawnUnit(UnitType.Blaster);
            }
        }
    }
}