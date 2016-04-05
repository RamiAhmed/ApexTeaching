namespace Apex.AI.NeuralNetwork
{
    public class Synapse
    {
        public Neuron inputNeuron;
        public Neuron outputNeuron;
        public double weight;
        public double weightDelta;

        public Synapse(Neuron inputNeuron, Neuron outputNeuron)
        {
            this.inputNeuron = inputNeuron;
            this.outputNeuron = outputNeuron;
            this.weight = NeuralNetwork.NextRandom();
        }
    }
}