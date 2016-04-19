namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class AttackTargetTypeScorer : OptionScorerBase<ICanDie>
    {
        [ApexSerialization]
        public float score;

        [ApexSerialization]
        public bool onlyNest;

        [ApexSerialization, MemberDependency("onlyNest", true)]
        public UnitType unitType;

        public override float Score(IAIContext context, ICanDie option)
        {
            if (this.onlyNest)
            {
                if (option is NestStructure)
                {
                    return this.score;
                }
            }

            var unit = option.gameObject.GetComponent<UnitBase>();
            if (unit != null)
            {
                if (unit.type == this.unitType)
                {
                    return this.score;
                }
            }

            return 0f;
        }
    }
}