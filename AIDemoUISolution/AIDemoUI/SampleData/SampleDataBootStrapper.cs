using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using DeepLearningDataProvider;
using NeuralNetBuilder.FactoriesAndParameters;
using System.Collections.ObjectModel;

namespace AIDemoUI.SampleData
{
    public class SampleDataBootStrapper// : SampleDataBootStrapper
    {
        #region fields & ctors

        static SampleDataBootStrapper()
        {
            var container = DIContainerManager.Container;
            container.Resolve<SampleDataBootStrapper>();
        }
        public SampleDataBootStrapper(
            INetParameters netParameters, ITrainerParameters trainerParameters, ILayerParameters layerParameters,
            IMainWindowVM mainWindowVM, INetParametersVM netParametersVM, IStartStopVM startStopVM, IStatusVM statusVM, 
            ObservableCollection<ILayerParameters> layerParametersCollection, 
            ILayerParametersFactory layerParametersFactory, ISamplesSteward samplesSteward,
            SampleImportWindow sampleImportWindow, ISimpleMediator mediator)
        {
            //SampleSessionContext = sessionContext;
            SampleMainWindowVM = mainWindowVM;
            SampleNetParameters = netParameters;
            SampleTrainerParameters = trainerParameters;
            SampleLayerParameters = layerParameters;
            SampleLayerParametersFactory = layerParametersFactory;
            SampleNetParametersVM = netParametersVM;
            SampleStartStopVM = startStopVM;
            SampleStatusVM = statusVM;
            SampleLayerParametersCollection = layerParametersCollection;
            SampleSampleImportWindow = sampleImportWindow;
            SampleMediator = mediator;
            SampleSamplesSteward = samplesSteward;
        }

        #endregion

        #region public

        //public static ISessionContext SampleSessionContext { get; set; }
        public static IMainWindowVM SampleMainWindowVM { get; set; }
        public static INetParameters SampleNetParameters { get; set; }
        public static ITrainerParameters SampleTrainerParameters { get; set; }
        public static ILayerParameters SampleLayerParameters { get; set; }
        public static INetParametersVM SampleNetParametersVM { get; set; }
        public static IStartStopVM SampleStartStopVM { get; set; }
        public static IStatusVM SampleStatusVM { get; set; }
        public static ObservableCollection<ILayerParameters> SampleLayerParametersCollection { get; set; }
        public static SampleImportWindow SampleSampleImportWindow { get; set; }  // use a adelegate?
        public static ISimpleMediator SampleMediator { get; set; }
        public static ILayerParametersFactory SampleLayerParametersFactory { get; set; }
        public static ISamplesSteward SampleSamplesSteward { get; set; }

        #endregion
    }
}