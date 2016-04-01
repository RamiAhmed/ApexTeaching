namespace Apex.AI.Teaching
{
    using NeuralNetwork;
    using UnityEngine;

    public sealed class NeuralNetworkLoadComponent : ExtendedMonoBehaviour
    {
        public TextAsset networkText;

        protected override void OnStartAndEnable()
        {
            if (this.networkText == null || this.networkText.text.Length == 0)
            {
                Debug.LogWarning(this.ToString() + " could not load a NeuralNetwork from file: " + networkText);
                return;
            }

            var network = NeuralNetworkHelper.LoadNetwork(this.networkText);
            Debug.Log("Loaded network: " + network);
        }
    }
}