namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class FSMBlasterAIComponent : AIComponentBase<BlasterUnit>
    {
        private enum BlasterState
        {
            Idle,
            Exploding
        }

        [SerializeField]
        private BlasterState _currentState = BlasterState.Idle;

        private ICanDie _attackTarget;

        protected override void ExecuteAI()
        {
            if (_currentState == BlasterState.Idle)
            {
                HandleIdle();
            }
            else if (_currentState == BlasterState.Exploding)
            {
                HandleExploding();
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

                var nest = obs.GetComponent<NestStructure>();
                if (nest != null)
                {
                    if (_entity.IsAllied(nest))
                    {
                        // try to avoid attacking own nest
                        continue;
                    }

                    _attackTarget = nest;
                    _currentState = BlasterState.Exploding;
                    return;
                }

                var otherUnit = obs.GetComponent<UnitBase>();
                if (otherUnit != null)
                {
                    if (_entity.IsAllied(otherUnit))
                    {
                        // try to avoid attacking allied units
                        continue;
                    }

                    _attackTarget = otherUnit;
                    _currentState = BlasterState.Exploding;
                    return;
                }
            }

            // nothing interesting in memory, do some random wandering
            if (!_entity.isMoving)
            {
                _entity.RandomWander();
            }
        }

        private void HandleExploding()
        {
            if (_attackTarget == null || _attackTarget.isDead)
            {
                _attackTarget = null;
                _currentState = BlasterState.Idle;
                return;
            }

            if ((_attackTarget.transform.position - _entity.transform.position).sqrMagnitude > (_entity.explodeRadius * _entity.explodeRadius))
            {
                // attack target outside of range
                if (!_entity.isMoving)
                {
                    _entity.MoveTo(_attackTarget.transform.position);
                }

                return;
            }

            // attack target within range
            _entity.Attack();
        }
    }
}