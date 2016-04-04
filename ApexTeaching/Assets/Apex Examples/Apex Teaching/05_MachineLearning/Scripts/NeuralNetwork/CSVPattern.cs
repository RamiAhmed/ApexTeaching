using System;

namespace Apex.AI.NeuralNetwork
{
    public class CSVPattern
    {
        public double[] inputs;
        public double output;

        public CSVPattern(string value, int inputSize)
        {
            string[] line = value.Split(',');
            if (line.Length == 0)
            {
                throw new ArgumentNullException("value");
            }
            
            if (inputSize > line.Length)
            {
                throw new Exception("Too many input dimensions defined, only " + line.Length + " lines found");
            }

            this.inputs = new double[inputSize];
            for (int i = 0; i < inputSize; i++)
            {
                this.inputs[i] = double.Parse(line[i]);
            }

            this.output = double.Parse(line[inputSize]);
        }
    }
}