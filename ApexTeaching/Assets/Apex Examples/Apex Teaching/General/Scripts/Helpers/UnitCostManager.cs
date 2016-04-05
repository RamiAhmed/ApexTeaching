namespace Apex.AI.Teaching
{
    public static class UnitCostManager
    {
        private const int harvesterUnitCost = 50;
        private const int warriorUnitCost = 75;
        private const int blasterUnitcost = 120;

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
                    return warriorUnitCost;
                }

                case UnitType.Blaster:
                {
                    return blasterUnitcost;
                }
            }

            return 0;
        }
    }
}