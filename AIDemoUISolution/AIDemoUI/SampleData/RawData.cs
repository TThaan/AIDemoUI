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
            ILayerParametersVMFactory layerParametersVMFactory, ILayerParametersFactory layerParametersFactory, ISampleSetSteward sampleSetSteward,
            SampleImportWindow sampleImportWindow, ISimpleMediator mediator,
            INet net)
        {
            RawSessionContext = sessionContext;
            RawMainWindowVM = mainWindowVM;
            RawLayerParameters = layerParameters;
            RawLayerParametersVMFactory = layerParametersVMFactory;
            RawLayerParametersFactory = layerParametersFactory;
            RawNetParametersVM = netParametersVM;
            RawStartStopVM = startStopVM;
            RawStatusVM = statusVM;
            RawSampleImportWindow = sampleImportWindow;
            RawMediator = mediator;
            RawSampleSetSteward = sampleSetSteward;
            RawNet = net;
        }

        #endregion

        #region public

        public static ISessionContext RawSessionContext { get; set; }
        public static IMainWindowVM RawMainWindowVM { get; set; }
        public static ILayerParameters RawLayerParameters { get; set; }
        public static INetParametersVM RawNetParametersVM { get; set; }
        public static IStartStopVM RawStartStopVM { get; set; }
        public static IStatusVM RawStatusVM { get; set; }
        public static SampleImportWindow RawSampleImportWindow { get; set; }  // use a adelegate?
        public static ISimpleMediator RawMediator { get; set; }
        public static ILayerParametersVMFactory RawLayerParametersVMFactory { get; set; }
        public static ILayerParametersFactory RawLayerParametersFactory { get; set; }
        public static ISampleSetSteward RawSampleSetSteward { get; set; }
        public static INet RawNet { get; set; }

        #endregion
    }
}