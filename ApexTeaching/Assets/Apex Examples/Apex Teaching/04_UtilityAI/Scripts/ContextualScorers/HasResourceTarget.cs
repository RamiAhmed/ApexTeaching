namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    public sealed class HasResourceTarget : ContextualScorerBase
    {
        [ApexSerialization]
        public bool mustHaveResources;

        [ApexSerialization]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            if (c.resourceTarget != null)
            {
                if (this.mustHaveResources && c.resourceTarget.currentResources <= 0)
                {
                    return this.not ? this.score : 0f;
                }

                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}