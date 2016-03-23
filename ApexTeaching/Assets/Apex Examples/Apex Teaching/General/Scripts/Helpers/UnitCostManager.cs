namespace Apex.AI.Teaching
{
    public static class UnitCostManager
    {
        private const int harvesterUnitCost = 50;
        private const int fighterUnitCost = 75;
        private const int exploderUnitcost = 120;

        public static int GetCost(UnitType type)
        {
            switch (type)
            {
                case UnitType.Harvester:
                {
                    return harvesterUnitCost;
                }

                case UnitType.Warrior:
                {
                    return fighterUnitCost;
                }

                case UnitType.Blaster:
                {
                    return exploderUnitcost;
                }
            }

            return 0;
        }
    }
}