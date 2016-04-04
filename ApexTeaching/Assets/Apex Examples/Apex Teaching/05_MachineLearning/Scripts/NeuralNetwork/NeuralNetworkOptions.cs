namespace Apex.AI.NeuralNetwork
{
    public class NeuralNetworkOptions
    {
        public string outputFileName = string.Empty;
        public string outputFilePath = string.Empty;
        public string patternsText = string.Empty;
        public bool datasetHasHeaders = true;
        public int inputDimensions = 2;
        public int hiddenDimensions = 2;
        public int outputDimensions = 1;
        public int maxIterations = 1000;
        public double targetErrorRate = 0.1d;
        public float lambda = 6f;
        public float learnRate = 0.5f;
        public float trainingPercentage = 0.66f;
    }
}