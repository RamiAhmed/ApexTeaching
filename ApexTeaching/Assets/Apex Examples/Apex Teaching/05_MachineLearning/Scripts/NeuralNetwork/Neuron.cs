namespace Apex.AI.NeuralNetwork
{
    using System.Collections.Generic;
    using System.Linq;

    public class Neuron
    {
        public List<Synapse> inputSynapses;
        public List<Synapse> outputSynapses;
        public double bias;
        public double biasDelta;
        public double gradient;
        public double value;

        public Neuron()
        {
            this.inputSynapses = new List<Synapse>();
            this.outputSynapses = new List<Synapse>();
            this.bias = NeuralNetwork.NextRandom();
        }

        public Neuron(List<Neuron> inputNeurons)
            : this()
        {
            var count = inputNeurons.Count;
            for (int i = 0; i < count; i++)
            {
                var inputNeuron = inputNeurons[i];
                var synapse = new Synapse(inputNeuron, this);
                inputNeuron.outputSynapses.Add(synapse);
                this.inputSynapses.Add(synapse);
            }
        }

        public virtual double CalculateValue()
        {
            return (this.value = NeuralNetwork.SigmoidFunction(inputSynapses.Sum(a => a.weight * a.inputNeuron.value) + bias));
        }

        public virtual double CalculateDerivative()
        {
            return NeuralNetwork.SigmoidDerivative(value);
        }

        public double CalculateError(double target)
        {
            return target - value;
        }

        public double CalculateGradient(double target)
        {
            return (this.gradient = CalculateError(target) * CalculateDerivative());
        }

        public double CalculateGradient()
        {
            return (this.gradient = outputSynapses.Sum(a => a.outputNeuron.gradient * a.weight) * CalculateDerivative());
        }

        public void UpdateWeights(double learnRate, double momentum)
        {
            var prevDelta = biasDelta;
            biasDelta = learnRate * gradient;
            bias += biasDelta + momentum * prevDelta;

            var count = inputSynapses.Count;
            for (int i = 0; i < count; i++)
            {
                var synapse = inputSynapses[i];
                prevDelta = synapse.weightDelta;
                synapse.weightDelta = learnRate * gradient * synapse.inputNeuron.value;
                synapse.weight += synapse.weightDelta + momentum * prevDelta;
            }
        }
    }
}