namespace Apex.AI.NeuralNetwork
{
    using Apex.AI.Teaching;
    using UnityEngine;

    public class NeuralNetworkComponent : AIComponentBase<AIController>
    {
        public static NeuralNetwork neuralNetwork { get; private set; }

        [Header("Data Set"), Space]
        public TextAsset patternsFile = null;
        public bool datasetHasHeaders = true;
        public bool debugLogWhenTraining = false;

        [Range(0.1f, 1f)]
        public float trainingPercentage = 0.8f;

        [Header("Iterations"), Space]
        [Range(1, 100000)]
        public int iterationCount = 1000;

        [Range(1, 100)]
        public int networkCount = 10;

        [Header("Dimensions"), Space]
        [Range(1, 20)]
        public int inputs = 5;

        [Range(1, 20)]
        public int hidden = 5;

        [Range(1, 20)]
        public int output = 4;

        [Header("Hyper Parameters"), Space]
        [Range(0.01f, 0.99f)]
        public double learnRate = 0.65d;

        [Range(0.01f, 0.99f)]
        public double momentum = 0.04d;

        [Range(0.001f, 0.25f)]
        public double targetError = 0.075d;

        [Header("Online"), Space]
        [Range(0.01f, 0.99f)]
        public double outputThreshold = 0.6d;

        private void OnEnable()
        {
            if (neuralNetwork != null)
            {
                return;
            }

            var network = new NeuralNetworkManager();
            neuralNetwork = network.Run(this.patternsFile.text, this.iterationCount, this.networkCount, this.inputs, this.hidden, this.output, this.trainingPercentage, this.datasetHasHeaders, this.debugLogWhenTraining, this.learnRate, this.momentum, this.targetError);
        }

        protected override void ExecuteAI()
        {
            var nest = _entity.nest;
            var inputs = new double[]
            {
                nest.currentResources,
                Time.timeSinceLevelLoad,
                nest.harvesterCount,
                nest.warriorCount,
                nest.blasterCount
            };

            var outputs = neuralNetwork.Compute(inputs);
            for (int i = 0; i < outputs.Length; i++)
            {
                var output = outputs[i];
                if (output > this.outputThreshold)
                {
                    // success
                    switch (i)
                    {
                        case 0:
                        {
                            nest.SpawnUnit(UnitType.Harvester);
                            return;
                        }

                        case 1:
                        {
                            nest.SpawnUnit(UnitType.Blaster);
                            return;
                        }

                        case 2:
                        {
                            nest.SpawnUnit(UnitType.Warrior);
                            return;
                        }

                        case 3:
                        {
                            // Do nothing
                            return;
                        }
                    }
                }

                //Debug.Log("Output == " + outputs[i]);
            }
        }
    }
}