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

            if (nest.harvesterCount < this.desiredHarvesterCount)
            {
                nest.SpawnUnit(UnitType.Harvester);
            }
            else if (nest.warriorCount <= nest.blasterCount)
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