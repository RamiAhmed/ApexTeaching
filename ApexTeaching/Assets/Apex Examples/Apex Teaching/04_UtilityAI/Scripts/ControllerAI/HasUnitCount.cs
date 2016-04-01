namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    public sealed class HasUnitCount : ContextualScorerBase
    {
        [ApexSerialization]
        public UnitType unitType;

        [ApexSerialization]
        public int desiredCount = 10;

        [ApexSerialization]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (ControllerContext)context;
            var nest = c.controller.nest;
            var count = 0;
            switch (this.unitType)
            {
                case UnitType.Harvester:
                {
                    count = nest.harvesterCount;
                    break;
                }

                case UnitType.Warrior:
                {
                    count = nest.warriorCount;
                    break;
                }

                case UnitType.Blaster:
                {
                    count = nest.blasterCount;
                    break;
                }

                default:
                {
                    Debug.LogWarning(this.ToString() + " Unsupported unit type => " + this.unitType);
                    break;
                }
            }

            if (count >= this.desiredCount)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}