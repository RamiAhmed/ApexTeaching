namespace Apex.AI.Teaching
{
    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    public sealed class ResourceComponent : MonoBehaviour
    {
        private const int maxUnitsHarvesting = 6;

        public float harvestRadius = 2.5f;
        public int minResources = 100;
        public int maxResources = 1000;
        public int currentResources;
        public int maxResourcesPerHarvest = 6;

        private readonly Vector3[] _harvestPositions = new Vector3[maxUnitsHarvesting];

        private void OnEnable()
        {
            this.currentResources = Random.Range(this.minResources, this.maxResources);
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, Random.Range(0f, 360f), this.transform.eulerAngles.z);

            // Initialize all the harvester positions
            var angle = 360f / maxUnitsHarvesting;
            for (int i = 0; i < maxUnitsHarvesting; i++)
            {
                _harvestPositions[i] = CircleHelpers.GetPointOnCircle(this.transform.position, harvestRadius, angle, i);
            }
        }

        private void OnDisable()
        {
            Grid.instance.UpdateCellsBlockedStatus(this.GetComponent<Collider>());
        }

        public void Harvest(HarvesterUnit unit)
        {
            var resources = Random.Range(1, this.maxResourcesPerHarvest);
            this.currentResources -= resources;
            unit.currentCarriedResources += resources;

            if (this.currentResources <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Gets a position for harvesters to be in when harvesting.
        /// </summary>
        /// <returns>The nearest position in a circle around this resource</returns>
        public Vector3 GetHarvestingPosition(UnitBase unit)
        {
            var unitPos = unit.transform.position;
            var nearest = _harvestPositions[0];
            for (int i = 1; i < _harvestPositions.Length; i++)
            {
                var pos = _harvestPositions[i];
                if ((unitPos - pos).sqrMagnitude < (unitPos - nearest).sqrMagnitude)
                {
                    nearest = pos;
                }
            }

            return nearest;
        }
    }
}