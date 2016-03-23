namespace Apex.AI.Teaching
{
    using Apex.Serialization;

    /// <summary>
    /// Scorer for evaluating whether this unit has a resource target, optionally checking that the resource target actually has resources.
    /// </summary>
    /// <seealso cref="Apex.AI.ContextualScorerBase" />
    public sealed class HasResourceTarget : ContextualScorerBase
    {
        [ApexSerialization, FriendlyName("Must Have Resources", "If set to true, also checks that the resource target actually has more resources left")]
        public bool mustHaveResources = true;

        [ApexSerialization, FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;
            if (c.resourceTarget != null)
            {
                // unit has a resource target
                if (this.mustHaveResources && c.resourceTarget.currentResources <= 0)
                {
                    // resource target has no resources and we are checking
                    return this.not ? this.score : 0f;
                }

                // resource target is valid
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}