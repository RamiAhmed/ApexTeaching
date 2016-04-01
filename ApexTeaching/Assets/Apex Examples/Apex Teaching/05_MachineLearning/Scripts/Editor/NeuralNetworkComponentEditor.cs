namespace Apex.AI.Teaching.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(NeuralNetworkTrainingComponent))]
    public class NeuralNetworkComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var editedObject = this.serializedObject.targetObject;
            var nnComponent = editedObject as NeuralNetworkTrainingComponent;
            if (nnComponent == null)
            {
                return;
            }

            if (GUILayout.Button("Train Neural Network"))
            {
                nnComponent.Run();
            }
        }
    }
}