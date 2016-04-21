namespace Apex.AI.Teaching
{
    using UnityEngine;

    /// <summary>
    /// Interface for all steering components
    /// </summary>
    public interface ISteeringComponent
    {
        /// <summary>
        /// Gets the priority of this particular steering component. The priority controls whether this steering component is executed. Higher priority steering components get executed first, and the first one with a value is used.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        int priority { get; }

        /// <summary>
        /// Gets the steering vector, or null if no steering vector is output.
        /// </summary>
        /// <param name="input">The steering input, holding data such as different speeds.</param>
        /// <returns></returns>
        Vector3? GetSteering(SteeringInput input);
    }
}