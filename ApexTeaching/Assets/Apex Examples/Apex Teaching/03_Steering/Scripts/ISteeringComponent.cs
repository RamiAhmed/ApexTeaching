namespace Apex.AI.Teaching
{
    using UnityEngine;

    public interface ISteeringComponent
    {
        int priority { get; }

        Vector3? GetSteering(SteeringInput input);
    }
}