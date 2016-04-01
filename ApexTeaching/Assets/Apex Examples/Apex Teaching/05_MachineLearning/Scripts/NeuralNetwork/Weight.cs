namespace Apex.AI.NeuralNetwork
{
    using Apex.Serialization;
    
    public class Weight
    {
        [ApexSerialization]
        public Neuron input;

        [ApexSerialization]
        public double value;

        public Weight()
        {
        }

        public Weight(Neuron input, double value)
        {
            this.input = input;
            this.value = value;
        }
    }
}