namespace Apex.AI.NeuralNetwork
{
    using System;

    public class Pattern
    {
        private double[] _inputs;
        private double _output;

        public Pattern(string value, int inputSize)
        {
            string[] line = value.Split(',');
            if (line.Length - 1 != inputSize)
            {
                throw new Exception("Input does not match network configuration");
            }

            _inputs = new double[inputSize];
            for (int i = 0; i < inputSize; i++)
            {
                _inputs[i] = double.Parse(line[i]);
            }

            _output = double.Parse(line[inputSize]);
        }

        public double[] inputs
        {
            get { return _inputs; }
        }

        public double output
        {
            get { return _output; }
        }
    }
}