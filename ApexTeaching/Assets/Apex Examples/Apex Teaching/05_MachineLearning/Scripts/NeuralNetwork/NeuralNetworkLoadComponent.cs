namespace Apex.AI.NeuralNetwork
{
    using System.Text;
    using UnityEngine;

    public sealed class NeuralNetworkLoadComponent : ExtendedMonoBehaviour
    {
        public static NeuralNetworkLoadComponent instance { get; private set; }

        public double[] inputs = new double[0];
        public TextAsset networkText = null;

        public NeuralNetwork network
        {
            get;
            private set;
        }

        protected override void OnStartAndEnable()
        {
            if (this.networkText == null || this.networkText.text.Length == 0)
            {
                Debug.LogWarning(this.ToString() + " could not load a NeuralNetwork from file: " + networkText);
                return;
            }

            if (instance != null)
            {
                Destroy(instance, 0.1f);
                Debug.LogWarning(this.ToString() + " another NeuralNetworkLoadComponent has already been registered, destroying this one");
                return;
            }

            instance = this;

            Debug.Log("============= NEURAL NETWORK LOADING RESULTS =============");
            this.network = NeuralNetworkHelper.LoadNetwork(this.networkText);
            
            var outputLayer = this.network.RunOnlineMultiOutput(this.inputs);
            Debug.Log("Loaded network: " + network);

            var count = outputLayer.Count;
            for (int i = 0; i < count; i++)
            {
                var sb = new StringBuilder();
                for (int j = 0; j < this.inputs.Length; j++)
                {
                    sb.Append(this.inputs[j]);

                    if (j < this.inputs.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }

                Debug.Log((i + 1) + " - Input (" + sb.ToString() + ") => " + outputLayer[i].output);
            }
        }
    }
}