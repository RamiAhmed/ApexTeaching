namespace Apex.AI.Teaching
{
    using UnityEngine;

    [RequireComponent(typeof(UnitBase))]
    public sealed class AIScannerComponent : AIComponentBase<UnitBase>
    {
        protected override void ExecuteAI()
        {
            if (_entity != null && !_entity.isDead)
            {
                _entity.AddOrUpdateObservations(Physics.OverlapSphere(_entity.transform.position, _entity.scanRadius, Layers.all));

                var observations = _entity.observations;
                var count = observations.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    var obs = observations[i];
                    if (obs == null || !obs.gameObject.activeSelf)
                    {
                        observations.RemoveAt(i);
                        continue;
                    }

                    var unit = obs.GetComponent<UnitBase>();
                    if (unit != null && unit.isDead)
                    {
                        observations.RemoveAt(i);
                        continue;
                    }

                    var resource = obs.GetComponent<ResourceComponent>();
                    if (resource != null && resource.currentResources <= 0)
                    {
                        observations.RemoveAt(i);
                        continue;
                    }

                    var nest = obs.GetComponent<NestStructure>();
                    if (nest != null && nest.isDead)
                    {
                        observations.RemoveAt(i);
                    }
                }

                observations.Sort(new GameObjectDistanceSortComparer(this.transform.position));
            }
        }
    }
}