using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI.SampleData
{
    /// <summary>
    /// Raw dependency container objects to inject into sample data view models.
    /// </summary>
    public class RawData
    {
        #region fields & ctors

        static RawData()
        {
            new DIManager().Container.Resolve<RawData>();
        }

        public RawData(
            ISessionContext sessionContext, ILayerParameters layerParameters,
            IMainWindowVM mainWindowVM, INetParametersVM netParametersVM, IStartStopVM startStopVM, IStatusVM statusVM,
            ILayerParametersVMFactory layerParametersVMFactory, ILayerParametersFactory layerParametersFactory, ISamplesSteward samplesSteward,
            SampleImportWindow sampleImportWindow, ISimpleMediator mediator,
            INet net)
        {
            SampleSessionContext = sessionContext;
            SampleMainWindowVM = mainWindowVM;
            SampleLayerParameters = layerParameters;
            SampleLayerParametersVMFactory = layerParametersVMFactory;
            SampleLayerParametersFactory = layerParametersFactory;
            SampleNetParametersVM = netParametersVM;
            SampleStartStopVM = startStopVM;
            SampleStatusVM = statusVM;
            SampleSampleImportWindow = sampleImportWindow;
            SampleMediator = mediator;
            SampleSamplesSteward = samplesSteward;
            SampleNet = net;
        }

        #endregion

        #region public

        public static ISessionContext SampleSessionContext { get; set; }
        public static IMainWindowVM SampleMainWindowVM { get; set; }
        public static ILayerParameters SampleLayerParameters { get; set; }
        public static INetParametersVM SampleNetParametersVM { get; set; }
        public static IStartStopVM SampleStartStopVM { get; set; }
        public static IStatusVM SampleStatusVM { get; set; }
        public static SampleImportWindow SampleSampleImportWindow { get; set; }  // use a adelegate?
        public static ISimpleMediator SampleMediator { get; set; }
        public static ILayerParametersVMFactory SampleLayerParametersVMFactory { get; set; }
        public static ILayerParametersFactory SampleLayerParametersFactory { get; set; }
        public static ISamplesSteward SampleSamplesSteward { get; set; }
        public static INet SampleNet { get; set; }

        #endregion
    }
}