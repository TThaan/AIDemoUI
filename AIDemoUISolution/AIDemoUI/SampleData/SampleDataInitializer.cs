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
    /// Actually this only provides static properties to be used in VMSampleData constructors.
    /// They don't bear any default values.
    /// 
    /// wa: one class ViewModelsSampleData with parameterless ctor incl props ... nope..
    /// wa: prop = context.Resolve..?
    /// </summary>
    public class SampleDataInitializer// : ISampleDataInitializer
    {
        #region fields & ctors

        static SampleDataInitializer()
        {
            new DIManager().Container.Resolve<SampleDataInitializer>();
        }

        public SampleDataInitializer(
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
            SampleNet = net;    // redundant? Already injected in SampleSession..
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