namespace Apex.AI.NeuralNetwork
{
    using System;
    using System.Collections.Generic;
    using Apex.Serialization;
    
    public class Neuron
    {
        [ApexSerialization]
        private List<Weight> _weights;              // Collection of weights to inputs.

        private double _lambda;                     // Steepness of sigmoid curve.
        private double _learnRate;                  // Learning rate.
        private double _bias;                       // Bias value.
        private double _error;                      // Sum of error.
        private double _input;                      // Sum of inputs.
        private double _output = double.MinValue;   // Preset value of neuron.

        public Neuron()
        {
        }

        public Neuron(float lambda, float learnRate, Layer inputs, Random rnd)
            : this(lambda, learnRate)
        {
            var count = inputs.Count;
            _weights = new List<Weight>(count);

            for (int i = 0; i < count; i++)
            {
                var input = inputs[i];
                _weights.Add(new Weight(input, (rnd.NextDouble() * 2d) - 1d));
            }
        }

        public Neuron(float lambda, float learnRate)
        {
            _lambda = lambda;
            _learnRate = learnRate;
        }

        [ApexSerialization]
        public double output
        {
            get
            {
                if (_output != double.MinValue)
                {
                    return _output;
                }

                return 1d / (1d + Math.Exp(-_lambda * (_input + _bias)));
            }

            set
            {
                _output = value;
            }
        }

        private double derivative
        {
            get
            {
                var activation = this.output;
                return activation * (1d - activation);
            }
        }

        public void Activate()
        {
            _input = 0d;

            var count = _weights.Count;
            for (int i = 0; i < count; i++)
            {
                var w = _weights[i];
                _input += w.value * w.input.output;
            }
        }

        public double ErrorFeedback(Neuron input)
        {
            Weight w = null;
            var count = _weights.Count;
            for (int i = 0; i < count; i++)
            {
                if (_weights[i].input == input)
                {
                    w = _weights[i];
                    break;
                }
            }

            return _error * derivative * w.value;
        }

        public void AdjustWeights(double value)
        {
            _error = value;
            for (int i = 0; i < _weights.Count; i++)
            {
                _weights[i].value += _error * derivative * _learnRate * _weights[i].input.output;
            }

            _bias += _error * derivative * _learnRate;
        }

        public override string ToString()
        {
            return string.Concat(this.GetType().ToString(), ": Derivative: ", this.derivative, ", output: ", _output, "; ");
        }
    }
}