namespace Apex.AI.Teaching
{
    using UnityEngine;

    [RequireComponent(typeof(AIController))]
    public sealed class AIControllerVisualizer : MonoBehaviour
    {
        private const float width = 150f;
        private const float height = 100f;
        private const float padding = 5f;

        [SerializeField]
        private bool _leftAlign;

        private AIController _aiController;

        private void OnEnable()
        {
            _aiController = this.GetComponent<AIController>();
        }

        private void OnGUI()
        {
            if (!this.enabled || !Application.isPlaying)
            {
                return;
            }

            var nest = _aiController.nest;
            var text = string.Concat(
                "Resources: ", nest.currentResources.ToString(),
                "\nNest HP: ", nest.currentHealth.ToString("F0"), " / ", nest.maxHealth.ToString("F0"),
                "\nTotal Units: ", nest.units.Count.ToString(),
                "\nHarvesters: ", nest.harvesterCount,
                "\nWarriors: ", nest.warriorCount,
                "\nBlasters: ", nest.blasterCount);

            Rect rect;
            if (_leftAlign)
            {
                rect = new Rect(padding, padding, width, height);
            }
            else
            {
                rect = new Rect(Screen.width - width - (padding * 2f), padding, width, height);
            }

            GUI.color = _aiController.color;
            GUI.Box(rect, text);
        }
    }
}