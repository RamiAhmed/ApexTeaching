namespace Apex.AI.Teaching
{
    using Apex.Serialization;
    using UnityEngine;

    public sealed class CellUnitDistanceScorer : OptionScorerBase<Cell>
    {
        [ApexSerialization, MemberDependency("onlyAllies", false)]
        public bool onlyEnemies;

        [ApexSerialization, MemberDependency("onlyEnemies", false)]
        public bool onlyAllies;

        [ApexSerialization]
        public float maxScore;

        public override float Score(IAIContext context, Cell option)
        {
            var c = (AIContext)context;
            var observations = c.unit.observations;
            var count = observations.Count;
            if (count == 0)
            {
                return 0f;
            }

            var actualCount = 0;
            var distanceSummed = 0f;
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var obsUnit = obs.gameObject.GetComponent<UnitBase>();
                if (obsUnit != null)
                {
                    if (c.unit.IsAllied(obsUnit))
                    {
                        if (this.onlyEnemies)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (this.onlyAllies)
                        {
                            continue;
                        }
                    }
                }

                var obsNest = obs.gameObject.GetComponent<NestStructure>();
                if (obsNest != null)
                {
                    if (c.unit.IsAllied(obsNest))
                    {
                        if (this.onlyEnemies)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (this.onlyAllies)
                        {
                            continue;
                        }
                    }
                }

                distanceSummed += (c.unit.transform.position - option.position).sqrMagnitude;
                actualCount++;
            }

            distanceSummed /= actualCount;
            return Mathf.Max(this.maxScore, distanceSummed);
        }
    }
}