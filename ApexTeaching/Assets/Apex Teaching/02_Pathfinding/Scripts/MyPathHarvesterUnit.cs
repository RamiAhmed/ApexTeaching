namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class MyPathHarvesterUnit : HarvesterUnit
    {
        public override bool isMoving
        {
            // Implement logic for evaluating whether the unit is moving (e.g. if the path is not null)
            get { return base.isMoving; }
        }

        public override void MoveTo(Vector3 destination)
        {
            // Check that the destination is valid - get a valid destination if it is not - and issue a path request through SteerForPath
            base.MoveTo(destination);
        }

        public override void RandomWander()
        {
            // Generate a random location (see example class 'PathHarvesterUnit' for help) and issue a move to to that location
            base.RandomWander();
        }

        public override void StopMoving()
        {
            // stop all movement, e.g. by nulling the path
            base.StopMoving();
        }
    }
}