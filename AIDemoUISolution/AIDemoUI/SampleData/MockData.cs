using Autofac;
using DeepLearningDataProvider;
using DeepLearningDataProvider.FourPixCam;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI.SampleData
{
    /// <summary>
    /// Example objects defined with example values
    /// (exclusively used in sample data view models).
    /// </summary>
    public class MockData// : ISampleDataInitializer
    {
        #region fields & ctors

        private readonly static IContainer _container;

        private static ITrainerParameters mockTrainerParameters;
        private static ILayerParameters mockLayerParameters01, mockLayerParameters02, mockLayerParameters03;
        private static INetParameters mockNetParameters;

        private static ISessionContext mockSessionContext;
        // private static ISimpleMediator mockMediator;
        private static INet mockNet;
        private static ITrainer mockTrainer;
        private static SampleSet mockSampleSet;

        static MockData()
        {
            _container = new DIManager().Container;
        }

        #endregion

        #region public

        public static ISessionContext MockSessionContext => mockSessionContext ?? (mockSessionContext = GetMockSessionContext());
        // public static ISimpleMediator MockMediator => mockMediator ?? (mockMediator = GetMockMediator());
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
            ISessionContext result = _container.Resolve<ISessionContext>();

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
            ISimpleMediator result = _container.Resolve<ISimpleMediator>();

            return result;
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
            var result = _container.Resolve<ITrainerParameters>();

            result.LearningRate = .1f;
            result.LearningRateChange = .9f;
            result.Epochs = 10;
            result.CostType = CostType.SquaredMeanError;

            return result;
        }
        private static INet GetMockNet()
        {
            return Initializer.InitializeNet(Initializer.GetRawNet(), MockNetParameters);
        }
        private static INetParameters GetMockNetParameters()
        {
            INetParameters result = _container.Resolve<INetParameters>();

            // FileName = "DefaultFileName",
            result.LayerParametersCollection.Add(MockLayerParameters01);
            result.LayerParametersCollection.Add(MockLayerParameters02);
            result.LayerParametersCollection.Add(MockLayerParameters03);
            result.WeightInitType = WeightInitType.Xavier;

            return result;
        }
        private static ILayerParameters GetMockLayerParameters01()
        {
            var result = _container.Resolve<ILayerParameters>();

            result.Id = 0;
            result.NeuronsPerLayer = 6;
            result.ActivationType = ActivationType.NullActivator;
            result.BiasMin = 0;
            result.BiasMax = 0;
            result.WeightMin = -1;
            result.WeightMax = 1;

            return result;
        }
        private static ILayerParameters GetMockLayerParameters02()
        {
            var result = _container.Resolve<ILayerParameters>();

            result.Id = 1;
            result.NeuronsPerLayer = 12;
            result.ActivationType = ActivationType.Tanh;
            result.BiasMin = 0;
            result.BiasMax = 0;
            result.WeightMin = -1;
            result.WeightMax = 1;

            return result;
        }
        private static ILayerParameters GetMockLayerParameters03()
        {
            var result = _container.Resolve<ILayerParameters>();

            result.Id = 2;
            result.NeuronsPerLayer = 6;
            result.ActivationType = ActivationType.LeakyReLU;
            result.BiasMin = 0;
            result.BiasMax = 0;
            result.WeightMin = -1;
            result.WeightMax = 1;

            return result;
        }
        private static SampleSet GetMockSampleSet()
        {
            return new FourPixCamSampleSet()
            {
                Status = "SampleSet created."
            };
        }


        #endregion
    }
}
