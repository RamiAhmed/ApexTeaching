/*
 * Thanks to:
 * http://www.craigsprogramming.com/2014/01/simple-c-artificial-neural-network.html
 * */

namespace Apex.AI.NeuralNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public class NeuralNetworkManager
    {
        private List<NeuralNetwork> neuralNetworks = new List<NeuralNetwork>();
        private List<CSVPattern> _trainPatterns;    // Collection of training patterns.
        private List<CSVPattern> _testPatterns;     // Collection of test patterns.

        private void LoadPatterns(string patternsText, bool hasHeaders, float trainingPercentage, int inputSize, bool debugLog)
        {
            var lines = patternsText.Split('\n');
            var lineLength = hasHeaders ? lines.Length - 1 : lines.Length;

            var trainLength = (int)Math.Round(lineLength * trainingPercentage);
            _trainPatterns = new List<CSVPattern>(trainLength);

            var count = (hasHeaders ? trainLength + 1 : trainLength);
            for (int i = hasHeaders ? 1 : 0; i < count; i++)
            {
                _trainPatterns.Add(new CSVPattern(lines[i], inputSize));
            }

            var testLength = lineLength - trainLength;
            _testPatterns = new List<CSVPattern>(testLength);

            for (int j = 0; j < testLength; j++)
            {
                _testPatterns.Add(new CSVPattern(lines[testLength + j], inputSize));
            }

            if (debugLog)
            {
                Debug.Log("Network loaded: Training patterns == " + trainLength + ", Test patterns == " + testLength);
            }
        }

        public NeuralNetwork Run(string patternText, int countIterations, int countNetworks, int inputs, int hidden, int outputs, float trainingPercentage, bool datasetHasHeaders, bool debugLog, double learnRate, double momentum, double targetError)
        {
            NeuralNetwork best = null;
            double lowestError = double.MaxValue;
            double error = 0d;
            double totalError = 0d;
            double trainingError = 0d;
            int bestIteration = -1;

            if (debugLog)
            {
                Debug.Log(Time.time + " Training Networks...");
            }

            LoadPatterns(patternText, datasetHasHeaders, trainingPercentage, inputs, debugLog);

            for (int j = 0; j < countNetworks; j++)
            {
                totalError = 0d;

                var network = new NeuralNetwork(inputs, hidden, outputs, learnRate, momentum);
                neuralNetworks.Add(network);

                for (int i = 0; i < countIterations; i++)
                {
                    trainingError = 0d;

                    var trainCount = _trainPatterns.Count;
                    for (int h = 0; h < trainCount; h++)
                    {
                        network.Train(_trainPatterns[h].inputs);
                        network.BackPropagate(_trainPatterns[h].outputs);

                        trainingError += network.CalculateError(_trainPatterns[h].outputs);
                    }

                    trainingError /= trainCount;
                    if (trainingError < targetError)
                    {
                        if (debugLog)
                        {
                            Debug.Log("Training error (" + trainingError.ToString("F6") + ") is below target error (" + targetError.ToString("F6") + "), at iteration: " + i);
                        }

                        break;
                    }
                }

                var testCount = _testPatterns.Count;
                for (int i = 0; i < testCount; i++)
                {
                    var output = network.Compute(_testPatterns[i].inputs);
                    error = network.CalculateError(_testPatterns[i].outputs);
                    totalError += error;

                    if (debugLog)
                    {
                        Debug.Log("Target == (" + DoubleArrayToString(_testPatterns[i].outputs) + "), output == (" + DoubleArrayToString(output) + "), error == " + error.ToString("F6"));
                    }
                }

                totalError /= testCount;

                if (totalError < lowestError)
                {
                    lowestError = totalError;
                    best = network;
                    bestIteration = j;
                }

                if (debugLog)
                {
                    Debug.Log("Completed iteration == " + j + " lowestError == " + lowestError + " best network == " + bestIteration);
                }
            }

            totalError = 0d;

            if (debugLog)
            {
                Debug.Log(Time.time + " best iteration == " + bestIteration + " best error == " + lowestError);
            }

            return best;
        }

        private string DoubleArrayToString(double[] arr)
        {
            var sb = new StringBuilder(arr.Length * 2);

            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr[i].ToString("F6"));

                if (i < arr.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }
    }
}