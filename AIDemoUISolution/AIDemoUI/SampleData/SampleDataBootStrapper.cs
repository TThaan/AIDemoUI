using AIDemoUI.Factories;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using DeepLearningDataProvider;
using NeuralNetBuilder.FactoriesAndParameters;
using System.Collections.ObjectModel;

namespace AIDemoUI.SampleData
{
    public class SampleDataBootStrapper
    {
        public SampleDataBootStrapper(ISessionContext sessionContext,
            NetParameters netParameters, TrainerParameters trainerParameters, LayerParameters layerParameters,
            MainWindowVM mainWindowVM, NetParametersVM netParametersVM, StartStopVM startStopVM, StatusVM statusVM, ObservableCollection<LayerParametersVM> layerParametersVMCollection, 
            LayerParametersVMFactory layerParametersVMFactory, ISamplesSteward samplesSteward,
            SampleImportWindow sampleImportWindow)//
        {
            SampleSessionContext = sessionContext;
            SampleMainWindowVM = mainWindowVM;
            SampleNetParameters = netParameters;
            SampleTrainerParameters = trainerParameters;
            SampleLayerParameters = layerParameters;
            SampleLayerParametersVMFactory = layerParametersVMFactory;
            SampleNetParametersVM = netParametersVM;
            SampleStartStopVM = startStopVM;
            SampleStatusVM = statusVM;
            SampleLayerParametersVMCollection = layerParametersVMCollection;
            SampleSampleImportWindow = sampleImportWindow;
            SampleSamplesSteward = samplesSteward;
        }

        public static ISessionContext SampleSessionContext { get; set; }
        public static MainWindowVM SampleMainWindowVM { get; set; }
        public static INetParameters SampleNetParameters { get; set; }
        public static ITrainerParameters SampleTrainerParameters { get; set; }
        public static LayerParameters SampleLayerParameters { get; set; }
        public static NetParametersVM SampleNetParametersVM { get; set; }
        public static StartStopVM SampleStartStopVM { get; set; }
        public static StatusVM SampleStatusVM { get; set; }
        public static ObservableCollection<LayerParametersVM> SampleLayerParametersVMCollection { get; set; }
        public static SampleImportWindow SampleSampleImportWindow { get; set; }
        public static LayerParametersVMFactory SampleLayerParametersVMFactory { get; set; }
        public static ISamplesSteward SampleSamplesSteward { get; set; }
    }
}