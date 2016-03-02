namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class FSMExploderAIComponent : AIComponentBase<ExploderUnit>
    {
        private enum ExploderState
        {
            Idle,
            Exploding
        }

        [SerializeField]
        private ExploderState _currentState = ExploderState.Idle;

        private ICanDie _attackTarget;

        protected override void ExecuteAI()
        {
            if (_currentState == ExploderState.Idle)
            {
                HandleIdle();
            }
            else if (_currentState == ExploderState.Exploding)
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
                        // don't attack own nest
                        continue;
                    }

                    _attackTarget = nest;
                    _currentState = ExploderState.Exploding;
                    return;
                }

                var otherUnit = obs.GetComponent(typeof(UnitBase)) as UnitBase;
                if (otherUnit != null)
                {
                    if (_entity.IsAllied(otherUnit))
                    {
                        // don't attack allied units
                        continue;
                    }

                    _attackTarget = otherUnit;
                    _currentState = ExploderState.Exploding;
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
                _currentState = ExploderState.Idle;
                return;
            }

            if ((_attackTarget.transform.position - _entity.transform.position).sqrMagnitude > (_entity.attackRadius * _entity.attackRadius))
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