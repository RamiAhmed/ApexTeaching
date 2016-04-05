namespace Apex.AI.NeuralNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NeuralNetwork
    {
        public double LearnRate;
        public double Momentum;
        public List<Neuron> InputLayer;
        public List<Neuron> HiddenLayer;
        public List<Neuron> OutputLayer;

        private static System.Random random = new System.Random();

        public NeuralNetwork(int inputSize, int hiddenSize, int outputSize, double learnRate, double momentum)
        {
            LearnRate = learnRate;
            Momentum = momentum;
            InputLayer = new List<Neuron>();
            HiddenLayer = new List<Neuron>();
            OutputLayer = new List<Neuron>();

            for (int i = 0; i < inputSize; i++)
            {
                InputLayer.Add(new Neuron());
            }

            for (int i = 0; i < hiddenSize; i++)
            {
                HiddenLayer.Add(new Neuron(InputLayer));
            }

            for (int i = 0; i < outputSize; i++)
            {
                OutputLayer.Add(new Neuron(HiddenLayer));
            }
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
            return 2d * random.NextDouble() - 1;
        }

        public static double SigmoidFunction(double x)
        {
            if (x < -45.0d) return 0.0d;
            else if (x > 45.0d) return 1.0d;
            return 1.0d / (1.0d + Math.Exp(-x));
        }

        public static double SigmoidDerivative(double f)
        {
            return f * (1d - f);
        }
    }
}