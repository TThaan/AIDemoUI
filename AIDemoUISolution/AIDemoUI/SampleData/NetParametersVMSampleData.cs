using AIDemoUI.ViewModels;
using NeuralNetBuilder.FactoriesAndParameters;
using NeuralNetBuilder;
using static AIDemoUI.SampleData.SampleDataInitializer;

namespace AIDemoUI.SampleData
{
    public class NetParametersVMSampleData : NetParametersVM
    {
        #region ctor

        public NetParametersVMSampleData()
            : base(SampleSessionContext, SampleMediator, SampleLayerParametersVMFactory, SampleLayerParametersFactory)
        {
            // Better: Define SampleSessionContext
            // _trainerParameters = GetSampleTrainerParameters();
            // _netParameters = GetSampleNetParameters();

            AreParametersGlobal = true;
            WeightMin_Global = -1;
            WeightMax_Global = 1;
            BiasMin_Global = 0;
            BiasMax_Global = 0;

            // Define collection with three items, null-items are sufficient
            // since they only serve as dummies that the NetParametersView creates a LayerParametersView for
            // using LayerParametersVMSampleData as content (not the item). 

            //LayerParametersVMCollection = new ObservableCollection<ILayerParametersVM>(
                //new List<LayerParametersVM>() { null, null, null });


            // throw new ArgumentException($"NetParameters.LayerParametersCollection.Length: {NetParameters.LayerParametersCollection.Length}");
        }

        #endregion

        #region helpers

        private ITrainerParameters GetSampleTrainerParameters()
        {
            return new TrainerParameters()
            {
                LearningRate = .1f,
                LearningRateChange = .9f,
                Epochs = 10,
                CostType = CostType.SquaredMeanError
            };
        }
        private INetParameters GetSampleNetParameters()
        {
            var result = new NetParameters();

            // FileName = "DefaultFileName",
            result.LayerParametersCollection.Add(GetSampleLayerParameters01());
            result.LayerParametersCollection.Add(GetSampleLayerParameters02());
            result.LayerParametersCollection.Add(GetSampleLayerParameters03());
            // WeightInitType = WeightInitType.Xavier

            return result;
        }
        private ILayerParameters GetSampleLayerParameters01()
        {
            return new LayerParameters()
            {
                Id = 0,
                NeuronsPerLayer = 6,
                ActivationType = ActivationType.NullActivator,
                BiasMin = 0,
                BiasMax = 0,
                WeightMin = -2,
                WeightMax = 2,
            };
        }
        private ILayerParameters GetSampleLayerParameters02()
        {
            return new LayerParameters()
            {
                Id = 1,
                NeuronsPerLayer = 12,
                ActivationType = ActivationType.Tanh,
                BiasMin = 0,
                BiasMax = 0,
                WeightMin = -2,
                WeightMax = 2,
            };
        }
        private ILayerParameters GetSampleLayerParameters03()
        {
            return new LayerParameters()
            {
                Id = 2,
                NeuronsPerLayer = 6,
                ActivationType = ActivationType.LeakyReLU,
                BiasMin = 0,
                BiasMax = 0,
                WeightMin = -2,
                WeightMax = 2,
            };
        }

        #endregion
    }
}
