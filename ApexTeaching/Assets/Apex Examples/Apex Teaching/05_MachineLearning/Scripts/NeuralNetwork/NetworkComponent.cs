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

        private void Start()
        {
            var network = new Network();
            neuralNetwork = network.Run(this.patternsFile.text, this.iterationCount, this.networkCount, this.inputs, this.hidden, this.output, this.trainingPercentage, this.datasetHasHeaders, this.debugLogWhenTraining, this.learnRate, this.momentum, this.targetError);
        }

        protected override void ExecuteAI()
        {
            var inputs = new double[]
            {
                _entity.nest.currentResources,
                Time.timeSinceLevelLoad,
                _entity.nest.harvesterCount,
                _entity.nest.warriorCount,
                _entity.nest.blasterCount
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
                            _entity.nest.SpawnUnit(UnitType.Harvester);
                            break;
                        }

                        case 1:
                        {
                            _entity.nest.SpawnUnit(UnitType.Blaster);
                            break;
                        }

                        case 2:
                        {
                            _entity.nest.SpawnUnit(UnitType.Warrior);
                            break;
                        }

                        case 3:
                        {
                            // Do nothing
                            break;
                        }
                    }
                }

                //Debug.Log("Output == " + outputs[i]);
            }
        }
    }
}