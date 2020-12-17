using NeuralNetBuilder.FactoriesAndParameters;
using System;

namespace AIDemoUI
{
    [Serializable]
    public class SerializedParameters
    {
        public INetParameters NetParameters { get; set; }
        public ITrainerParameters TrainerParameters { get; set; }

        public static SerializedParameters GetDefautlTemplate()
        {
            SerializedParameters result = new SerializedParameters();

            result.NetParameters = new NetParameters()
            {

            };
            result.TrainerParameters = new TrainerParameters()
            {

            };

            return result;
        }
    }
}
