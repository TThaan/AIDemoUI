using AIDemoUI.Factories;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI.SampleData
{
    public class SampleDataBootStrapper
    {
        public SampleDataBootStrapper(ISessionContext sampleSesssionContext, NetParameters netParameters, TrainerParameters trainerParameters, LayerParametersVMFactory layerParametersVMFactory)//, NetParametersVM netParametersVM, StartStopVM startStopVM, StatusVM statusVM, SampleImportWindow sampleImportWindow
        {
            SampleSessionContext = sampleSesssionContext;
            SampleNetParameters = netParameters;
            SampleTrainerParameters = trainerParameters;
            SampleLayerParametersVMFactory = layerParametersVMFactory;
        }

        public static ISessionContext SampleSessionContext { get; set; }
        public static INetParameters SampleNetParameters { get; set; }
        public static ITrainerParameters SampleTrainerParameters { get; set; }
        public static NetParametersVM SampleNetParametersVM => SampleSessionContext.NetParametersVM;
        public static StartStopVM SampleStartStopVM => SampleSessionContext.StartStopVM;
        public static StatusVM SampleStatusVM => SampleSessionContext.StatusVM;
        public static SampleImportWindow SampleSampleImportWindow => SampleSessionContext.SampleImportWindow;
        public static LayerParametersVMFactory SampleLayerParametersVMFactory { get; set; }
    }
}