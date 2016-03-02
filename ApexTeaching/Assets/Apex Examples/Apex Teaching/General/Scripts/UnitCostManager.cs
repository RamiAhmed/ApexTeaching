namespace Apex.AI.Teaching
{
    public static class UnitCostManager
    {
        private const int harvesterUnitCost = 50;
        private const int fighterUnitCost = 75;
        private const int exploderUnitcost = 80;

        public static int GetCost(UnitType type)
        {
            switch (type)
            {
                case UnitType.Harvester:
                {
                    return harvesterUnitCost;
                }

                case UnitType.Fighter:
                {
                    return fighterUnitCost;
                }

                case UnitType.Exploder:
                {
                    return exploderUnitcost;
                }
            }

            return 0;
        }
    }
}