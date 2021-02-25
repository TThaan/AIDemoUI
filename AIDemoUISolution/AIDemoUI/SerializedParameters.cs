using NeuralNetBuilder.FactoriesAndParameters;
using System;

namespace AIDemoUI
{
    [Serializable]
    public class SerializedParameters// : ISerializedParameters
    {
        public INetParameters NetParameters { get; set; }
        public ITrainerParameters TrainerParameters { get; set; }
        //public static SerializedParameters GetDefautlTemplate()
        //{
        //    SerializedParameters result = new SerializedParameters();
        //
        //    result.NetParameters = new NetParameters()
        //    {
        //
        //    };
        //    result.TrainerParameters = new TrainerParameters()
        //    {
        //
        //    };
        //
        //    return result;
        //}
        public bool UseGlobalParameters { get; set; }
        public float WeightMin_Global { get; set; }
        public float WeightMax_Global { get; set; }
        public float BiasMin_Global { get; set; }
        public float BiasMax_Global { get; set; }
    }
}
