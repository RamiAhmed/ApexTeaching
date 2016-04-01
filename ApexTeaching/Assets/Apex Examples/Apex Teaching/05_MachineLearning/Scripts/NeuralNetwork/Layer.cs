namespace Apex.AI.NeuralNetwork
{
    using System;
    using System.Collections.Generic;
    using Apex.Serialization;

    [ApexSerializedType]
    public class Layer : List<Neuron>
    {
        public Layer(int capacity)
            : base(capacity)
        {
        }

        public Layer(float lambda, float learnRate, int size)
        {
            for (int i = 0; i < size; i++)
            {
                Add(new Neuron(lambda, learnRate));
            }
        }

        public Layer(float lambda, float learnRate, int size, Layer layer, Random rnd)
        {
            for (int i = 0; i < size; i++)
            {
                Add(new Neuron(lambda, learnRate, layer, rnd));
            }
        }
    }
}