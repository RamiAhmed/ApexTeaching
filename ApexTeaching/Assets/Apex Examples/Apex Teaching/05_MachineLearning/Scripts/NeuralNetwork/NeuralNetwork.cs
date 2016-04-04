namespace Apex.AI.NeuralNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Apex.Serialization;
    using UnityEngine;

    public class NeuralNetwork
    {
        [ApexSerialization]
        private int _hiddenDims;                 // Number of hidden neurons.

        [ApexSerialization]
        private int _inputDims;                  // Number of input neurons.

        [ApexSerialization]
        private int _outputDims;                 // Number of output neurons.

        [ApexSerialization]
        private Layer _hidden;                   // Collection of hidden neurons.

        [ApexSerialization]
        private Layer _inputs;                   // Collection of input neurons.

        [ApexSerialization]
        private Layer _outputs;                  // Output neurons.

        private List<CSVPattern> _trainPatterns;    // Collection of training patterns.
        private List<CSVPattern> _testPatterns;     // Collection of test patterns.
        private System.Random _rnd = new System.Random(); // Global random number generator.
        private int _iteration;                  // Current training iteration.

        public NeuralNetwork()
        {
        }

        public void TrainAndTest(NeuralNetworkOptions options)
        {
            TrainAndTest(options.patternsText, options.inputDimensions, options.hiddenDimensions, options.outputDimensions, options.maxIterations, options.targetErrorRate, options.lambda, options.learnRate, options.trainingPercentage, options.outputFilePath, options.datasetHasHeaders, options.outputFileName);
        }

        public void TrainAndTest(string patternsText, int inputDimensions, int hiddenDimensions, int outputDimensions, int maxIterations, double targetErrorRate, float lambda, float learnRate, float trainingPercentage, string outputFilePath, bool datasetHasHeaders, string fileName)
        {
            _hiddenDims = hiddenDimensions;
            _inputDims = inputDimensions;
            _outputDims = outputDimensions;

            Debug.Log("============= NEURAL NETWORK TRAINING RESULTS =============");
            LoadPatterns(patternsText, datasetHasHeaders, trainingPercentage);
            Initialise(lambda, learnRate);
            Train(targetErrorRate, maxIterations);
            Test();

            NeuralNetworkHelper.SaveNetwork(this, outputFilePath, fileName);
            Debug.Log(this.ToString());
        }

        public Layer RunOnlineMultiOutput(double[] inputs)
        {
            double outputSum;
            return RunOnlineMultiOutput(inputs, out outputSum);
        }

        public Layer RunOnlineMultiOutput(double[] inputs, out double outputSum)
        {
            outputSum = Activate(inputs, true);
            return _outputs;
        }

        private void LoadPatterns(string patternsText, bool hasHeaders, float trainingPercentage)
        {
            var lines = patternsText.Split('\n');

            var trainLength = (int)Math.Round(lines.Length * trainingPercentage);
            _trainPatterns = new List<CSVPattern>(hasHeaders ? trainLength - 1 : trainLength);

            for (int i = hasHeaders ? 1 : 0; i < trainLength; i++)
            {
                _trainPatterns.Add(new CSVPattern(lines[i], _inputDims));
            }

            var testLength = (int)(lines.Length - trainLength);
            if (hasHeaders)
            {
                trainLength -= 1;
                testLength -= 1;
            }

            if (testLength > 0)
            {
                _testPatterns = new List<CSVPattern>(testLength);

                for (int j = 0; j < testLength; j++)
                {
                    _testPatterns.Add(new CSVPattern(lines[testLength + j], _inputDims));
                }
            }
            else
            {
                testLength = 0;
                _testPatterns = new List<CSVPattern>(0);
            }

            Debug.Log("Network loaded: Training patterns == " + trainLength + ", Test patterns == " + testLength);
        }

        private void Initialise(float lambda, float learnRate)
        {
            _inputs = new Layer(lambda, learnRate, _inputDims);
            _hidden = new Layer(lambda, learnRate, _hiddenDims, _inputs, _rnd);
            _outputs = new Layer(lambda, learnRate, _outputDims, _hidden, _rnd);
            _iteration = 0;
        }

        private void AdjustWeights(double delta)
        {
            var hiddenCount = _hidden.Count;
            var outputCount = _outputs.Count;
            for (int i = 0; i < outputCount; i++)
            {
                var output = _outputs[i];
                output.AdjustWeights(delta);

                for (int j = 0; j < hiddenCount; j++)
                {
                    var neuron = _hidden[j];
                    neuron.AdjustWeights(output.ErrorFeedback(neuron));
                }
            }
        }

        private void Train(double targetError, int maxIterations)
        {
            double error;
            do
            {
                error = 0d;

                var count = _trainPatterns.Count;
                for (int i = 0; i < count; i++)
                {
                    var pattern = _trainPatterns[i];
                    var delta = pattern.output - Activate(pattern);

                    AdjustWeights(delta);
                    error += Math.Pow(delta, 2d);
                }

                _iteration++;
            } while (error > targetError && _iteration < maxIterations);

            // For debugging individual training result comparisons
            for (int j = 0; j < _trainPatterns.Count; j++)
            {
                Debug.Log(j + ": Input == " + _trainPatterns[j].output + ", output => " + Activate(_trainPatterns[j]));
            }

            Debug.Log("Network trained: Error == " + error.ToString("F5") + ", iterations == " + _iteration);
        }

        private void Test()
        {
            int count = _testPatterns.Count;
            if (count == 0)
            {
                Debug.LogWarning("Neural network cannot run a test without any test patterns.");
                return;
            }

            var avgError = 0d;
            for (int i = 0; i < count; i++)
            {
                var pattern = _testPatterns[i];
                avgError += (pattern.output - Activate(pattern));
            }

            avgError /= count;

            Debug.Log("Network test: Average error == " + avgError.ToString("F5"));
        }

        private double Activate(CSVPattern pattern)
        {
            return Activate(pattern.inputs);
        }

        private double Activate(double[] inputs, bool remapConnections = false)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                _inputs[i].output = inputs[i];
            }

            var count = _hidden.Count;
            for (int i = 0; i < count; i++)
            {
                if (remapConnections)
                {
                    _hidden[i].RemapConnections(_inputs);
                }

                _hidden[i].Activate();
            }

            var summedOutput = 0d;
            count = _outputs.Count;
            for (int i = 0; i < count; i++)
            {
                if (remapConnections)
                {
                    _outputs[i].RemapConnections(_hidden);
                }

                _outputs[i].Activate();
                summedOutput += _outputs[i].output;
            }

            return summedOutput;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(this.GetType().ToString());

            var count = _inputs.Count;
            sb.Append(string.Concat("Input layer (", count, " dimension(s)): "));
            for (int i = 0; i < count; i++)
            {
                sb.Append(_inputs[i].ToString());
            }

            sb.AppendLine();

            count = _hidden.Count;
            sb.Append(string.Concat("Hidden layer (", count, " dimension(s)): "));
            for (int i = 0; i < count; i++)
            {
                sb.Append(_hidden[i].ToString());
            }

            sb.AppendLine();

            count = _outputs.Count;
            sb.Append(string.Concat("Output layer (", count, " dimension(s)): "));
            for (int i = 0; i < count; i++)
            {
                sb.Append(_outputs[i].ToString());
            }

            return sb.ToString();
        }
    }
}