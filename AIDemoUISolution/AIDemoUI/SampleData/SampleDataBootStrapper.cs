using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using DeepLearningDataProvider;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.ObjectModel;

namespace AIDemoUI.SampleData
{
    public class SampleDataBootStrapper
    {
        #region fields & ctors

        private static SampleDataBootStrapper instance;

        static SampleDataBootStrapper()
        {
            var container = DIContainerManager.Container;
            // var netParams = scope.Resolve<INetParameters>();
            // throw new ArgumentException($"Location: {typeof(SampleDataBootStrapper).Name}.StaticCtor\n{nameof(netParams)} = null: {netParams == null}");
            instance = container.Resolve<SampleDataBootStrapper>();
        }
        public SampleDataBootStrapper(ISessionContext sessionContext,
            INetParameters netParameters, ITrainerParameters trainerParameters, ILayerParameters layerParameters,
            MainWindowVM mainWindowVM, NetParametersVM netParametersVM, StartStopVM startStopVM, StatusVM statusVM, ObservableCollection<LayerParametersVM> layerParametersVMCollection, 
            ILayerParametersVMFactory layerParametersVMFactory, ISamplesSteward samplesSteward,
            SampleImportWindow sampleImportWindow, SimpleMediator mediator)
        {
            // throw new ArgumentException($"Location: {GetType().Name}:\n{nameof(sessionContext)} = null: {sessionContext == null}");
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
            SampleMediator = mediator;
            SampleSamplesSteward = samplesSteward;
        }

        #endregion

        #region public

        public static ISessionContext SampleSessionContext { get; set; }
        public static MainWindowVM SampleMainWindowVM { get; set; }
        public static INetParameters SampleNetParameters { get; set; }
        public static ITrainerParameters SampleTrainerParameters { get; set; }
        public static ILayerParameters SampleLayerParameters { get; set; }
        public static NetParametersVM SampleNetParametersVM { get; set; }
        public static StartStopVM SampleStartStopVM { get; set; }
        public static StatusVM SampleStatusVM { get; set; }
        public static ObservableCollection<LayerParametersVM> SampleLayerParametersVMCollection { get; set; }
        public static SampleImportWindow SampleSampleImportWindow { get; set; }
        public static SimpleMediator SampleMediator { get; set; }
        public static ILayerParametersVMFactory SampleLayerParametersVMFactory { get; set; }
        public static ISamplesSteward SampleSamplesSteward { get; set; }

        #endregion
    }
}