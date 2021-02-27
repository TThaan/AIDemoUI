using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using DeepLearningDataProvider;
using DeepLearningDataProvider.FourPixCam;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;

namespace AIDemoUI.SampleData
{
    /// <summary>
    /// Actually this only provides static properties to be used in VMSampleData constructors.
    /// They don't bear any default values.
    /// 
    /// wa: one class ViewModelsSampleData with parameterless ctor incl props ... nope..
    /// wa: prop = context.Resolve..?
    /// </summary>
    public class MockData// : ISampleDataInitializer
    {
        #region fields & ctors

        // private readonly static IComponentContext _context;
        private readonly static IContainer _context;    // container

        private static ITrainerParameters mockTrainerParameters;
        private static ILayerParameters mockLayerParameters01, mockLayerParameters02, mockLayerParameters03;
        private static INetParameters mockNetParameters;

        private static ISessionContext mockSessionContext;
        private static ISimpleMediator mockMediator;
        private static INet mockNet;
        private static ITrainer mockTrainer;
        private static SampleSet mockSampleSet;

        static MockData()
        {
            _context = new DIManager().Container;
        }

        #endregion

        #region public

        public static ISessionContext MockSessionContext => mockSessionContext ?? (mockSessionContext = GetMockSessionContext());
        public static ISimpleMediator MockMediator => mockMediator ?? (mockMediator = GetMockMediator());
        //public static IMainWindowVM SampleMainWindowVM { get; set; }
        //public static ILayerParameters SampleLayerParameters { get; set; }
        //public static INetParametersVM SampleNetParametersVM { get; set; }
        //public static IStartStopVM SampleStartStopVM { get; set; }
        //public static IStatusVM SampleStatusVM { get; set; }
        //public static SampleImportWindow SampleSampleImportWindow { get; set; }  // use a adelegate?
        //public static ILayerParametersVMFactory SampleLayerParametersVMFactory { get; set; }
        //public static ILayerParametersFactory SampleLayerParametersFactory { get; set; }
        //public static ISamplesSteward SampleSamplesSteward { get; set; }
        //public static INet SampleNet { get; set; }
        public static INetParameters MockNetParameters => mockNetParameters ?? (mockNetParameters = GetMockNetParameters());
        public static ILayerParameters MockLayerParameters01 => mockLayerParameters01 ?? (mockLayerParameters01 = GetMockLayerParameters01());
        public static ILayerParameters MockLayerParameters02 => mockLayerParameters02 ?? (mockLayerParameters02 = GetMockLayerParameters02());
        public static ILayerParameters MockLayerParameters03 => mockLayerParameters03 ?? (mockLayerParameters03 = GetMockLayerParameters03());
        public static ITrainerParameters MockTrainerParameters => mockTrainerParameters ?? (mockTrainerParameters = GetMockTrainerParameters());
        public static string MockFileName => "";
        public static INet MockNet => mockNet ?? (mockNet = GetMockNet());
        public static ITrainer MockTrainer => mockTrainer ?? (mockTrainer = GetMockTrainer());
        public static SampleSet MockSampleSet => mockSampleSet ?? (mockSampleSet = GetMockSampleSet());
        
        #endregion

        #region helpers

        private static ISessionContext GetMockSessionContext()
        {
            ISessionContext result = _context.Resolve<ISessionContext>();

            result.NetParameters = MockNetParameters;
            result.TrainerParameters = MockTrainerParameters;
            result.SampleSet = MockSampleSet;

            result.Net = MockNet;
            result.Trainer = MockTrainer;

            result.IsNetInitialized = true;
            result.IsTrainerInitialized = true;
            result.IsSampleSetInitialized = true;

            return result;
        }
        private static ISimpleMediator GetMockMediator()
        {
            ISimpleMediator result = _context.Resolve<ISimpleMediator>();

            return result;
        }
        private static INet GetMockNet()
        {
            return Initializer.InitializeNet(Initializer.GetRawNet(), MockNetParameters);
        }
        private static ITrainer GetMockTrainer()
        {
            ITrainer result = Initializer.InitializeTrainer(Initializer.GetRawTrainer(), MockNet, MockTrainerParameters, MockSampleSet);

            result.CurrentEpoch = 3;
            result.CurrentSample = 742;
            result.Epochs = 10;
            result.SamplesTotal = 1000;
            result.CurrentTotalCost = .00037647f;
            result.LastEpochsAccuracy = 0.625f;

            result.Status = "Training...";

            return result;
        }
        private static ITrainerParameters GetMockTrainerParameters()
        {
            var result = _context.Resolve<ITrainerParameters>();

            result.LearningRate = .1f;
            result.LearningRateChange = .9f;
            result.Epochs = 10;
            result.CostType = CostType.SquaredMeanError;

            return result;
        }
        private static SampleSet GetMockSampleSet()
        {
            return new FourPixCamSampleSet()
            {
                Status = "SampleSet created."
            };
        }
        private static INetParameters GetMockNetParameters()
        {
            INetParameters result = _context.Resolve<INetParameters>();

            // FileName = "DefaultFileName",
            result.LayerParametersCollection.Add(MockLayerParameters01);
            result.LayerParametersCollection.Add(MockLayerParameters02);
            result.LayerParametersCollection.Add(MockLayerParameters03);
            // WeightInitType = WeightInitType.Xavier

            return result;
        }
        private static ILayerParameters GetMockLayerParameters01()
        {
            var result = _context.Resolve<ILayerParameters>();

            result.Id = 0;
            result.NeuronsPerLayer = 6;
            result.ActivationType = ActivationType.NullActivator;
            result.BiasMin = 0;
            result.BiasMax = 0;
            result.WeightMin = -2;
            result.WeightMax = 2;

            return result;
        }
        private static ILayerParameters GetMockLayerParameters02()
        {
            var result = _context.Resolve<ILayerParameters>();

            result.Id = 1;
            result.NeuronsPerLayer = 12;
            result.ActivationType = ActivationType.Tanh;
            result.BiasMin = 0;
            result.BiasMax = 0;
            result.WeightMin = -2;
            result.WeightMax = 2;

            return result;
        }
        private static ILayerParameters GetMockLayerParameters03()
        {
            var result = _context.Resolve<ILayerParameters>();

            result.Id = 2;
            result.NeuronsPerLayer = 6;
            result.ActivationType = ActivationType.LeakyReLU;
            result.BiasMin = 0;
            result.BiasMax = 0;
            result.WeightMin = -2;
            result.WeightMax = 2;

            return result;
        }


        #endregion
    }
}
