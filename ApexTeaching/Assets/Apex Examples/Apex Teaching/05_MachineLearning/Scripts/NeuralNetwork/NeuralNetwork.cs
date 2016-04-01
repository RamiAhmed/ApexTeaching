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
        private int _hiddenDims;            // Number of hidden neurons.

        [ApexSerialization]
        private int _inputDims;             // Number of input neurons.

        [ApexSerialization]
        private int _outputDims;            // Number of output neurons.

        [ApexSerialization]
        private Layer _hidden;              // Collection of hidden neurons.

        [ApexSerialization]
        private Layer _inputs;              // Collection of input neurons.

        [ApexSerialization]
        private Layer _outputs;              // Output neurons.

        private int _iteration;             // Current training iteration.
        private int _restartAfter;          // Restart training if iterations exceed this.
        private List<Pattern> _patterns;    // Collection of training patterns.
        private List<Pattern> _testPatterns;// Collection of test patterns.
        private System.Random _rnd = new System.Random(); // Global random number generator.
        private float _trainingPercentage;
        private float _learnRate;
        private float _lambda;

        public NeuralNetwork()
        {
        }

        public void Run(string patternsText, int hiddenDimensions, int inputDimensions, int outputDimensions, int maxIterations, float lambda, float learnRate, float trainingPercentage, string outputFilePath)
        {
            _lambda = lambda;
            _learnRate = learnRate;
            _hiddenDims = hiddenDimensions;
            _inputDims = inputDimensions;
            _outputDims = outputDimensions;
            _restartAfter = maxIterations;
            _trainingPercentage = trainingPercentage;

            Debug.Log("=========== NEURAL NETWORK RESULTS ===========");
            LoadPatterns(patternsText);
            Initialise();
            Train();
            Test();

            NeuralNetworkHelper.SaveNetwork(this, outputFilePath);
        }

        private void LoadPatterns(string patternsText)
        {
            var lines = patternsText.Split('\n');

            var trainLength = (int)Math.Round(lines.Length * _trainingPercentage);
            _patterns = new List<Pattern>(trainLength);

            for (int i = 0; i < trainLength; i++)
            {
                _patterns.Add(new Pattern(lines[i], _inputDims));
            }

            var testLength = (int)(lines.Length - trainLength);
            _testPatterns = new List<Pattern>(testLength);

            for (int j = 0; j < testLength; j++)
            {
                _testPatterns.Add(new Pattern(lines[testLength + j], _inputDims));
            }

            Debug.Log("Network loaded: \nTraining patterns: " + trainLength + "\nTest patterns: " + testLength);
        }

        private void Initialise()
        {
            _inputs = new Layer(_lambda, _learnRate, _inputDims);
            _hidden = new Layer(_lambda, _learnRate, _hiddenDims, _inputs, _rnd);
            _outputs = new Layer(_lambda, _learnRate, _outputDims, _hidden, _rnd);
            _iteration = 0;
        }

        private void Train()
        {
            double error;
            do
            {
                error = 0d;

                var count = _patterns.Count;
                for (int i = 0; i < count; i++)
                {
                    var pattern = _patterns[i];
                    var delta = pattern.output - Activate(pattern);

                    AdjustWeights(delta);
                    error += Math.Pow(delta, 2d);
                }

                _iteration++;
            } while (error > 0.1d && _iteration < _restartAfter);

            Debug.Log("Network trained: Error == " + error.ToString("F5") + ", iterations == " + _iteration);
        }

        private void Test()
        {
            double avgError = 0d;

            int count = _testPatterns.Count;
            for (int i = 0; i < count; i++)
            {
                var pattern = _testPatterns[i];
                avgError += (pattern.output - Activate(pattern));
            }

            avgError /= count;

            Debug.Log("Network test: Average error: " + avgError.ToString("F5"));
        }

        private double Activate(Pattern pattern)
        {
            for (int i = 0; i < pattern.inputs.Length; i++)
            {
                _inputs[i].output = pattern.inputs[i];
            }

            var count = _hidden.Count;
            for (int i = 0; i < count; i++)
            {
                _hidden[i].Activate();
            }

            var summedOutput = 0d;
            count = _outputs.Count;
            for (int i = 0; i < count; i++)
            {
                _outputs[i].Activate();
                summedOutput += _outputs[i].output;
            }

            return summedOutput;
        }

        private void AdjustWeights(double delta)
        {
            var outputCount = _outputs.Count;
            for (int i = 0; i < outputCount; i++)
            {
                _outputs[i].AdjustWeights(delta);

                var count = _hidden.Count;
                for (int j = 0; j < count; j++)
                {
                    var neuron = _hidden[j];
                    neuron.AdjustWeights(_outputs[i].ErrorFeedback(neuron));
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(this.GetType().ToString());

            var count = _inputs.Count;
            sb.Append(string.Concat("Input layer (", count, "): "));
            for (int i = 0; i < count; i++)
            {
                sb.Append(_inputs[i].ToString());
            }

            sb.AppendLine();

            count = _hidden.Count;
            sb.Append(string.Concat("Hidden layer (", count, "): "));
            for (int i = 0; i < count; i++)
            {
                sb.Append(_hidden[i].ToString());
            }

            sb.AppendLine();

            count = _outputs.Count;
            sb.Append(string.Concat("Output layer (", count, "): "));
            for (int i = 0; i < count; i++)
            {
                sb.Append(_outputs[i].ToString());
            }

            return sb.ToString();
        }
    }
}