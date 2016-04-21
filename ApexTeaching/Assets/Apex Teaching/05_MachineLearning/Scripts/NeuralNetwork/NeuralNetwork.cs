namespace Apex.AI.NeuralNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NeuralNetwork
    {
        public double learnRate;
        public double momentum;
        public List<Neuron> inputLayer;
        public List<Neuron> hiddenLayer;
        public List<Neuron> outputLayer;

        private static readonly Random random = new Random();

        public NeuralNetwork(int inputSize, int hiddenSize, int outputSize, double learnRate, double momentum)
        {
            this.learnRate = learnRate;
            this.momentum = momentum;
            this.inputLayer = new List<Neuron>();
            this.hiddenLayer = new List<Neuron>();
            this.outputLayer = new List<Neuron>();

            for (int i = 0; i < inputSize; i++)
            {
                this.inputLayer.Add(new Neuron());
            }

            for (int i = 0; i < hiddenSize; i++)
            {
                this.hiddenLayer.Add(new Neuron(inputLayer));
            }

            for (int i = 0; i < outputSize; i++)
            {
                this.outputLayer.Add(new Neuron(hiddenLayer));
            }
        }

        public void Train(params double[] inputs)
        {
            int i = 0;
            this.inputLayer.ForEach(a => a.value = inputs[i++]);
            this.hiddenLayer.ForEach(a => a.CalculateValue());
            this.outputLayer.ForEach(a => a.CalculateValue());
        }

        public double[] Compute(params double[] inputs)
        {
            Train(inputs);
            return this.outputLayer.Select(a => a.value).ToArray();
        }

        public double CalculateError(params double[] targets)
        {
            int i = 0;
            return this.outputLayer.Sum(a => Math.Abs(a.CalculateError(targets[i++])));
        }

        public void BackPropagate(params double[] targets)
        {
            int i = 0;
            this.outputLayer.ForEach(a => a.CalculateGradient(targets[i++]));
            this.hiddenLayer.ForEach(a => a.CalculateGradient());
            this.hiddenLayer.ForEach(a => a.UpdateWeights(learnRate, momentum));
            this.outputLayer.ForEach(a => a.UpdateWeights(learnRate, momentum));
        }

        public static double NextRandom()
        {
            return (2d * random.NextDouble()) - 1;
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