namespace Apex.AI.NeuralNetwork
{
    public class Synapse
    {
        public Neuron InputNeuron;
        public Neuron OutputNeuron;
        public double Weight;
        public double WeightDelta;

        public Synapse(Neuron inputNeuron, Neuron outputNeuron)
        {
            InputNeuron = inputNeuron;
            OutputNeuron = outputNeuron;
            Weight = NeuralNetwork.NextRandom();
        }
    }
}