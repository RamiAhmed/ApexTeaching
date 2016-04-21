namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    public sealed class UnitCountContextualScorer : IContextualScorer
    {
        [ApexSerialization]
        public UnitType unitType;

        [ApexSerialization]
        public float maxScore = 100f;

        public bool isDisabled
        {
            get;
            set;
        }

        public float Score(IAIContext context)
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

            return Mathf.Max(0f, this.maxScore - count);
        }
    }
}