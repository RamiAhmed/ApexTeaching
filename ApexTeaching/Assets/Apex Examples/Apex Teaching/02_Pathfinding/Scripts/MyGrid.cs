namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class MyGrid : Grid
    {
        // Uncomment next line to use MyPathfinder
        //private readonly MyPathfinder _pathfinder = new MyPathfinder();

        protected override void OnEnable()
        {
            base.OnEnable();
            // (Optionally) Implement own initialization - remember to comment/remove base.OnEnable()
        }

        public override Path FindPath(Vector3 start, Vector3 destination)
        {
            // Use MyPathfinder by uncommenting the very next line, but removing/commenting the one after
            //return _pathfinder.FindPath(this, start, destination);
            return base.FindPath(start, destination);
        }
    }
}