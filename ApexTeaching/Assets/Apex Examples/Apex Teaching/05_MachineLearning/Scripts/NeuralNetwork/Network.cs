/*
 * Thanks to:
 * http://www.craigsprogramming.com/2014/01/simple-c-artificial-neural-network.html
 * */

namespace Apex.AI.NeuralNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public class Network
    {
        private List<NeuralNetwork> neuralNetworks = new List<NeuralNetwork>();
        private List<CSVPattern> _trainPatterns;    // Collection of training patterns.
        private List<CSVPattern> _testPatterns;     // Collection of test patterns.

        private void LoadPatterns(string patternsText, bool hasHeaders, float trainingPercentage, int inputSize, bool debugLog)
        {
            var lines = patternsText.Split('\n');
            var lineLength = hasHeaders ? lines.Length - 1 : lines.Length;

            var trainLength = (int)Math.Round(lineLength * trainingPercentage);
            _trainPatterns = new List<CSVPattern>(trainLength);

            var count = (hasHeaders ? trainLength + 1 : trainLength);
            for (int i = hasHeaders ? 1 : 0; i < count; i++)
            {
                _trainPatterns.Add(new CSVPattern(lines[i], inputSize));
            }

            var testLength = lineLength - trainLength;
            _testPatterns = new List<CSVPattern>(testLength);

            for (int j = 0; j < testLength; j++)
            {
                _testPatterns.Add(new CSVPattern(lines[testLength + j], inputSize));
            }

            if (debugLog)
            {
                Debug.Log("Network loaded: Training patterns == " + trainLength + ", Test patterns == " + testLength);
            }
        }

        public NeuralNetwork Run(string patternText, int countIterations, int countNetworks, int inputs, int hidden, int outputs, float trainingPercentage, bool datasetHasHeaders, bool debugLog, double learnRate, double momentum, double targetError)
        {
            NeuralNetwork best = null;
            double lowestError = double.MaxValue;
            double error = 0d;
            //double output = 0d;
            double totalError = 0d;
            double trainingError = 0d;
            int bestIteration = -1;

            if (debugLog)
            {
                Debug.Log(Time.time + " Training Networks...");
            }

            LoadPatterns(patternText, datasetHasHeaders, trainingPercentage, inputs, debugLog);

            for (int j = 0; j < countNetworks; j++)
            {
                totalError = 0d;

                var network = new NeuralNetwork(inputs, hidden, outputs, learnRate, momentum);
                neuralNetworks.Add(network);

                for (int i = 0; i < countIterations; i++)
                {
                    trainingError = 0d;

                    var trainCount = _trainPatterns.Count;
                    for (int h = 0; h < trainCount; h++)
                    {
                        network.Train(_trainPatterns[h].inputs);
                        network.BackPropagate(_trainPatterns[h].outputs);

                        trainingError += network.CalculateError(_trainPatterns[h].outputs);
                    }

                    trainingError /= trainCount;
                    if (trainingError < targetError)
                    {
                        Debug.Log("Training error (" + trainingError.ToString("F6") + ") is below target error (" + targetError.ToString("F6") + "), at iteration: " + i);
                        break;
                    }
                }

                var testCount = _testPatterns.Count;
                for (int i = 0; i < testCount; i++)
                {
                    var output = network.Compute(_testPatterns[i].inputs);
                    error = network.CalculateError(_testPatterns[i].outputs);
                    totalError += error;

                    if (debugLog)
                    {
                        Debug.Log("Target == (" + DoubleArrayToString(_testPatterns[i].outputs) + "), output == (" + DoubleArrayToString(output) + "), error == " + error.ToString("F6"));
                    }
                }

                totalError /= testCount;

                if (totalError < lowestError)
                {
                    lowestError = totalError;
                    best = network;
                    bestIteration = j;
                }

                if (debugLog)
                {
                    Debug.Log("Completed iteration == " + j + " lowestError == " + lowestError + " best network == " + bestIteration);
                }
            }

            totalError = 0d;

            if (debugLog)
            {
                Debug.Log(Time.time + " best iteration == " + bestIteration + " best error == " + lowestError);
            }

            return best;
        }

        private string DoubleArrayToString(double[] arr)
        {
            var sb = new StringBuilder(arr.Length * 2);

            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr[i].ToString("F6"));

                if (i < arr.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }
    }

    public class NeuralNetwork
    {
        public double LearnRate { get; set; }
        public double Momentum { get; set; }
        public List<Neuron> InputLayer { get; set; }
        public List<Neuron> HiddenLayer { get; set; }
        public List<Neuron> OutputLayer { get; set; }
        private static System.Random random = new System.Random();

        public NeuralNetwork(int inputSize, int hiddenSize, int outputSize, double learnRate, double momentum)
        {
            LearnRate = learnRate;
            Momentum = momentum;
            InputLayer = new List<Neuron>();
            HiddenLayer = new List<Neuron>();
            OutputLayer = new List<Neuron>();

            for (int i = 0; i < inputSize; i++)
                InputLayer.Add(new Neuron());

            for (int i = 0; i < hiddenSize; i++)
                HiddenLayer.Add(new Neuron(InputLayer));

            for (int i = 0; i < outputSize; i++)
                OutputLayer.Add(new Neuron(HiddenLayer));
        }

        public void Train(params double[] inputs)
        {
            int i = 0;
            InputLayer.ForEach(a => a.Value = inputs[i++]);
            HiddenLayer.ForEach(a => a.CalculateValue());
            OutputLayer.ForEach(a => a.CalculateValue());
        }

        public double[] Compute(params double[] inputs)
        {
            Train(inputs);
            return OutputLayer.Select(a => a.Value).ToArray();
        }

        public double CalculateError(params double[] targets)
        {
            int i = 0;
            return OutputLayer.Sum(a => Math.Abs(a.CalculateError(targets[i++])));
        }

        public void BackPropagate(params double[] targets)
        {
            int i = 0;
            OutputLayer.ForEach(a => a.CalculateGradient(targets[i++]));
            HiddenLayer.ForEach(a => a.CalculateGradient());
            HiddenLayer.ForEach(a => a.UpdateWeights(LearnRate, Momentum));
            OutputLayer.ForEach(a => a.UpdateWeights(LearnRate, Momentum));
        }

        public static double NextRandom()
        {
            return 2 * random.NextDouble() - 1;
        }

        public static double SigmoidFunction(double x)
        {
            if (x < -45.0) return 0.0;
            else if (x > 45.0) return 1.0;
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        public static double SigmoidDerivative(double f)
        {
            return f * (1 - f);
        }
    }

    public class Neuron
    {
        public List<Synapse> InputSynapses { get; set; }
        public List<Synapse> OutputSynapses { get; set; }
        public double Bias { get; set; }
        public double BiasDelta { get; set; }
        public double Gradient { get; set; }
        public double Value { get; set; }

        public Neuron()
        {
            InputSynapses = new List<Synapse>();
            OutputSynapses = new List<Synapse>();
            Bias = NeuralNetwork.NextRandom();
        }

        public Neuron(List<Neuron> inputNeurons)
            : this()
        {
            foreach (var inputNeuron in inputNeurons)
            {
                var synapse = new Synapse(inputNeuron, this);
                inputNeuron.OutputSynapses.Add(synapse);
                InputSynapses.Add(synapse);
            }
        }

        public virtual double CalculateValue()
        {
            return Value = NeuralNetwork.SigmoidFunction(InputSynapses.Sum(a => a.Weight * a.InputNeuron.Value) + Bias);
        }

        public virtual double CalculateDerivative()
        {
            return NeuralNetwork.SigmoidDerivative(Value);
        }

        public double CalculateError(double target)
        {
            return target - Value;
        }

        public double CalculateGradient(double target)
        {
            return Gradient = CalculateError(target) * CalculateDerivative();
        }

        public double CalculateGradient()
        {
            return Gradient = OutputSynapses.Sum(a => a.OutputNeuron.Gradient * a.Weight) * CalculateDerivative();
        }

        public void UpdateWeights(double learnRate, double momentum)
        {
            var prevDelta = BiasDelta;
            BiasDelta = learnRate * Gradient; // * 1
            Bias += BiasDelta + momentum * prevDelta;

            foreach (var s in InputSynapses)
            {
                prevDelta = s.WeightDelta;
                s.WeightDelta = learnRate * Gradient * s.InputNeuron.Value;
                s.Weight += s.WeightDelta + momentum * prevDelta;
            }
        }
    }

    public class Synapse
    {
        public Neuron InputNeuron { get; set; }
        public Neuron OutputNeuron { get; set; }
        public double Weight { get; set; }
        public double WeightDelta { get; set; }

        public Synapse(Neuron inputNeuron, Neuron outputNeuron)
        {
            InputNeuron = inputNeuron;
            OutputNeuron = outputNeuron;
            Weight = NeuralNetwork.NextRandom();
        }
    }
}