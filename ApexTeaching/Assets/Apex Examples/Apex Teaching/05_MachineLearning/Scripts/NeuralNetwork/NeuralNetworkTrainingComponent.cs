namespace Apex.AI.NeuralNetwork
{
    using UnityEngine;

    public sealed class NeuralNetworkTrainingComponent : MonoBehaviour
    {
        [Header("Files")]
        public string outputFileName = string.Empty;
        public string outputFilePath = string.Empty;
        public TextAsset patternsFile = null;
        public bool datasetHasHeaders = true;

        [Header("Dimensions")]
        [Range(1, 20)]
        public int inputDimensions = 2;

        [Range(1, 20)]
        public int hiddenDimensions = 2;

        [Range(1, 20)]
        public int outputDimensions = 1;

        [Header("Iterations")]
        [Range(1, 100)]
        public int repeatIterations = 1;

        [Range(100, 100000)]
        public int maxIterations = 1000;

        [Header("Hyper parameters")]
        [Range(0.01f, 0.99f)]
        public double targetErrorRate = 0.1d;

        [Range(0f, 1f)]
        public float learnRate = 0.5f;

        [Range(1f, 20f)]
        public float lambda = 6f;

        [Header("Training set to test set ratio")]
        [Range(0f, 1f)]
        public float trainingSetPercentage = 0.66f;

        private readonly NeuralNetwork _network = new NeuralNetwork();

        public void TrainAndTest()
        {
            if (this.patternsFile == null)
            {
                Debug.LogError(this.ToString() + " Cannot run network without a patterns file");
                return;
            }

            var options = new NeuralNetworkOptions()
            {
                inputDimensions = this.inputDimensions,
                hiddenDimensions = this.hiddenDimensions,
                outputDimensions = this.outputDimensions,
                maxIterations = this.maxIterations,
                targetErrorRate = this.targetErrorRate,
                learnRate = this.learnRate,
                lambda = this.lambda,
                trainingPercentage = this.trainingSetPercentage,
                datasetHasHeaders = this.datasetHasHeaders,
                outputFileName = this.outputFileName,
                outputFilePath = this.outputFilePath,
                patternsText = this.patternsFile.text,
            };

            for (int i = 0; i < this.repeatIterations; i++)
            {
                _network.TrainAndTest(options);
            }
        }
    }
}