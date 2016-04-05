namespace Apex.AI.NeuralNetwork
{
    using System.Collections.Generic;
    using System.Linq;

    public class Neuron
    {
        public List<Synapse> InputSynapses;
        public List<Synapse> OutputSynapses;
        public double Bias;
        public double BiasDelta;
        public double Gradient;
        public double Value;

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

            var count = InputSynapses.Count;
            for (int i = 0; i < count; i++)
            {
                var synapse = InputSynapses[i];
                prevDelta = synapse.WeightDelta;
                synapse.WeightDelta = learnRate * Gradient * synapse.InputNeuron.Value;
                synapse.Weight += synapse.WeightDelta + momentum * prevDelta;
            }
        }
    }
}