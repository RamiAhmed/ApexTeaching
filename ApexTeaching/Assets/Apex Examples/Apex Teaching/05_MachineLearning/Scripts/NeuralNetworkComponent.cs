namespace Apex.AI.Teaching
{
    using NeuralNetwork;
    using UnityEngine;

    public class NeuralNetworkComponent : AIComponentBase<AIController>
    {
        public NeuralNetwork neuralNetwork { get; private set; }

        [Header("Data Set")]
        public TextAsset patternsFile = null;

        public bool datasetHasHeaders = true;
        public bool debugLogWhenTraining = false;

        [Range(0.1f, 1f)]
        public float trainingPercentage = 0.8f;

        [Header("Iterations")]
        [Range(1, 100000)]
        public int iterationCount = 1000;

        [Range(1, 100)]
        public int networkCount = 10;

        [Header("Dimensions")]
        [Range(1, 20)]
        public int inputDimensions = 5;

        [Range(1, 20)]
        public int hiddenDimensions = 5;

        [Range(1, 20)]
        public int outputDimensions = 4;

        [Header("Hyper Parameters")]
        [Range(0.01f, 0.99f)]
        public double learnRate = 0.65d;

        [Range(0.01f, 0.99f)]
        public double momentum = 0.04d;

        [Range(0.001f, 0.25f)]
        public double targetError = 0.075d;

        [Header("Online")]
        [Range(0.01f, 0.99f)]
        public double outputThreshold = 0.6d;

        private void OnEnable()
        {
            if (neuralNetwork != null)
            {
                return;
            }

            if (this.patternsFile == null)
            {
                Debug.LogWarning(this.ToString() + " cannot run neural networks without a patterns file");
                return;
            }

            var network = new NeuralNetworkManager();
            neuralNetwork = network.Run(this.patternsFile.text, this.iterationCount, this.networkCount, this.inputDimensions, this.hiddenDimensions, this.outputDimensions, this.trainingPercentage, this.datasetHasHeaders, this.debugLogWhenTraining, this.learnRate, this.momentum, this.targetError);
        }

        protected override void ExecuteAI()
        {
            if (this.patternsFile == null)
            {
                return;
            }

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