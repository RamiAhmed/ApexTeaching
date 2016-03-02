namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class AIControllerVisualizer : MonoBehaviour
    {
        public bool leftAlign;

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

            var text = string.Concat("Resources: ", _aiController.currentResources.ToString(), "\nUnits: ", _aiController.units.Count);

            Rect rect;
            if (this.leftAlign)
            {
                rect = new Rect(5f, 5f, 100f, 60f);
            }
            else
            {
                rect = new Rect(Screen.width - 110f, 5f, 100f, 60f);
            }

            GUI.color = _aiController.color;
            GUI.Box(rect, text);
        }
    }
}