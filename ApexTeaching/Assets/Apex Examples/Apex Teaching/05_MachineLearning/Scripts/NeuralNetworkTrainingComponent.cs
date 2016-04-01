namespace Apex.AI.Teaching
{
    using NeuralNetwork;
    using UnityEngine;

    public sealed class NeuralNetworkTrainingComponent : MonoBehaviour
    {
        public string outputFilePath;
        public TextAsset patternsFile;

        [Range(1, 20)]
        public int inputDimensions = 2;

        [Range(1, 20)]
        public int hiddenDimensions = 2;

        [Range(1, 20)]
        public int outputDimensions = 1;

        [Range(1, 100)]
        public int repeatIterations = 1;

        [Range(100, 100000)]
        public int maxIterations = 1000;

        [Range(0f, 1f)]
        public float learnRate = 0.5f;

        [Range(1f, 20f)]
        public float lambda = 6f;

        [Range(0f, 1f)]
        public float trainingSetPercentage = 0.66f;

        private readonly NeuralNetwork _network = new NeuralNetwork();

        public void Run()
        {
            if (this.patternsFile == null)
            {
                Debug.LogError(this.ToString() + " Cannot run network without a patterns file");
                return;
            }

            for (int i = 0; i < this.repeatIterations; i++)
            {
                _network.Run(this.patternsFile.text, this.hiddenDimensions, this.inputDimensions, this.outputDimensions, this.maxIterations, this.lambda, this.learnRate, this.trainingSetPercentage, this.outputFilePath);
            }
        }
    }
}