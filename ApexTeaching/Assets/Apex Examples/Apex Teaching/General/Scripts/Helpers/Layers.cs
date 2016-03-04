namespace Apex.AI.Teaching
{
    using UnityEngine;

    public static class Layers
    {
        public static LayerMask units = 1 << LayerMask.NameToLayer("Units");
        public static LayerMask structures = 1 << LayerMask.NameToLayer("Structures");
        public static LayerMask resources = 1 << LayerMask.NameToLayer("Resources");
        public static LayerMask all = units | structures | resources;
    }
}