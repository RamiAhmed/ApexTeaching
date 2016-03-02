namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class FSMHarvesterAIComponent : AIComponentBase<HarvesterUnit>
    {
        private enum HarvesterState
        {
            Idle,
            Harvesting,
            Returning,
            Fleeing,
        }

        [SerializeField]
        private HarvesterState _currentState = HarvesterState.Idle;

        private UnitBase _fleeTarget;
        private ResourceComponent _resourceTarget;

        protected override void ExecuteAI()
        {
            if (_currentState == HarvesterState.Idle)
            {
                HandleIdle();
            }
            else if (_currentState == HarvesterState.Fleeing)
            {
                HandleFleeing();
            }
            else if (_currentState == HarvesterState.Harvesting)
            {
                HandleHarvesting();
            }
            else if (_currentState == HarvesterState.Returning)
            {
                HandleReturning();
            }
        }

        private void HandleReturning()
        {
            var distance = (_entity.transform.position - _entity.nest.transform.position).sqrMagnitude;
            if (distance > (_entity.returnHarvestRadius * _entity.returnHarvestRadius))
            {
                // nest ouside of range
                if (!_entity.isMoving)
                {
                    _entity.MoveTo(_entity.nest.transform.position);
                }

                return;
            }

            // nest inside range
            _entity.nest.controller.currentResources += _entity.currentCarriedResources;
            _entity.currentCarriedResources = 0;
            _currentState = HarvesterState.Idle;
        }

        private void HandleHarvesting()
        {
            if (_resourceTarget == null || _resourceTarget.currentResources <= 0)
            {
                // no more resources
                _currentState = HarvesterState.Idle;
                _resourceTarget = null;
                return;
            }

            var distance = (_entity.transform.position - _resourceTarget.transform.position).sqrMagnitude;
            if (distance > (_entity.attackRadius * _entity.attackRadius))
            {
                // resource out of range
                if (!_entity.isMoving)
                {
                    _entity.MoveTo(_resourceTarget.transform.position);
                }

                return;
            }

            // resource within range
            _entity.Harvest(_resourceTarget);
            if (_entity.currentCarriedResources >= _entity.maxCarriableResources)
            {
                // harvester cannot carry more resources
                _currentState = HarvesterState.Returning;
            }
        }

        private void HandleFleeing()
        {
            var fleeDir = (_entity.transform.position - _fleeTarget.transform.position);
            if (_fleeTarget == null || _fleeTarget.isDead || fleeDir.sqrMagnitude > (_entity.fleeRadius * _entity.fleeRadius))
            {
                // flee target is null, dead or outside of flee range
                _currentState = HarvesterState.Idle;
                _fleeTarget = null;
                return;
            }

            if (!_entity.isMoving)
            {
                var fleePos = _entity.transform.position + fleeDir.normalized * _entity.fleeRadius;
                _entity.MoveTo(fleePos);
            }
        }

        private void HandleIdle()
        {
            var observations = _entity.observations;
            observations.Sort(new GameObjectDistanceSortComparer(this.transform.position));

            var count = observations.Count;
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];

                var otherUnit = obs.GetComponent(typeof(UnitBase)) as UnitBase;
                if (otherUnit != null)
                {
                    if (_entity.IsAllied(otherUnit))
                    {
                        // don't flee from allies
                        continue;
                    }

                    if ((otherUnit.transform.position - _entity.transform.position).sqrMagnitude > (_entity.fleeRadius * _entity.fleeRadius))
                    {
                        // enemy is outside of flee radius
                        continue;
                    }

                    _fleeTarget = otherUnit;
                    _currentState = HarvesterState.Fleeing;
                    return;
                }

                var resource = obs.GetComponent<ResourceComponent>();
                if (resource != null)
                {
                    _resourceTarget = resource;
                    _currentState = HarvesterState.Harvesting;
                    return;
                }
            }

            // nothing interesting in memory, do some random wandering
            if (!_entity.isMoving)
            {
                _entity.RandomWander();
            }
        }
    }
}