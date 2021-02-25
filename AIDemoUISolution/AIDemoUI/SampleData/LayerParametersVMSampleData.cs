using AIDemoUI.ViewModels;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using static AIDemoUI.SampleData.SampleDataInitializer;

namespace AIDemoUI.SampleData
{
    public class LayerParametersVMSampleData : LayerParametersVM
    {
        public LayerParametersVMSampleData()
            : base(SampleSessionContext, SampleMediator)//
        {
            //throw new System.ArgumentException($"LayerParameters == null: {LayerParameters == null}");
            LayerParameters = new LayerParameters();
            // Id = 16;
            NeuronsPerLayer = 6;
            ActivationType = ActivationType.SoftMaxWithCrossEntropyLoss;
            WeightMin = -.8f;
            WeightMax = .8f;
        }
    }
}
