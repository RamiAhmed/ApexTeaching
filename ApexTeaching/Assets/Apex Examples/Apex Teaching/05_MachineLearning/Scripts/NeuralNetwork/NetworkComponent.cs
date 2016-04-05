namespace Apex.AI.NeuralNetwork
{
    using Apex.AI.Teaching;
    using UnityEngine;

    public class NetworkComponent : AIComponentBase<AIController>
    {
        public static NeuralNetwork neuralNetwork { get; private set; }

        public TextAsset patternsFile;
        public int iterationCount = 1000;
        public int networkCount = 10;
        public int inputs = 2;
        public int hidden = 2;
        public int output = 1;
        public float trainingPercentage = 0.8f;
        public bool datasetHasHeaders = true;
        public bool debugLogWhenTraining = false;
        public double learnRate = 0.65d;
        public double momentum = 0.04d;
        public double targetError = 0.05d;

        private void OnEnable()
        {
            if (neuralNetwork == null)
            {
                var network = new Network();
                neuralNetwork = network.Run(this.patternsFile.text, this.iterationCount, this.networkCount, this.inputs, this.hidden, this.output, this.trainingPercentage, this.datasetHasHeaders, this.debugLogWhenTraining, this.learnRate, this.momentum, this.targetError);
            }
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
                if (output > 0.5f)
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