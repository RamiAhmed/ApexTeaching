namespace Apex.AI.Teaching
{
    using UnityEngine;

    public static class CircleHelpers
    {
        public static Vector3 GetPointOnCircle(Vector3 position, float radius, float anglePerSpawn, int index)
        {
            var max = 360f / anglePerSpawn;
            var ang = (index % max) * anglePerSpawn;
            return new Vector3(
                    position.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad),
                    position.y,
                    position.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad));
        }
    }
}