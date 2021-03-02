using CustomLogger;
using DeepLearningDataProvider;
using MatrixHelper;
using NeuralNetBuilder;
using NeuralNetBuilder.ActivatorFunctions;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AIDemoUI.SampleData.MockData
{
    public class MockSessionContext : ISessionContext
    {
        public INet Net { get => new MockNet(); set => throw new NotImplementedException(); }
        public INetParameters NetParameters { get => new MockNetParameters(); set => throw new NotImplementedException(); }
        public ITrainer Trainer { get => new MockTrainer(); set => throw new NotImplementedException(); }
        public ITrainerParameters TrainerParameters { get => new MockTrainerParameters(); set => throw new NotImplementedException(); }
        public ISampleSet SampleSet { get => new MockSampleSet(); set => throw new NotImplementedException(); }
        public ISampleSetSteward SampleSetSteward => new MockSampleSetSteward();
        public bool IsNetInitialized { get => true; set => throw new NotImplementedException(); }
        public bool IsSampleSetInitialized { get => true; set => throw new NotImplementedException(); }
        public bool IsTrainerInitialized { get => true; set => throw new NotImplementedException(); }
    }
    public class MockNet : INet
    {
        public ILayer[] Layers
        {
            get => new ILayer[]
            {
                new MockLayer(1), new MockLayer(2), new MockLayer(3), new MockLayer(4), new MockLayer(5)
            };
            set => throw new NotImplementedException();
        }
        public IMatrix Output => throw new NotImplementedException();
        public string LoggableName => throw new NotImplementedException();
        public Task FeedForwardAsync(IMatrix input)
        {
            throw new NotImplementedException();
        }
        public INet GetCopy()
        {
            throw new NotImplementedException();
        }
        public string ToLog()
        {
            throw new NotImplementedException();
        }
        public NetStatus NetStatus { get => NetStatus.Initialized; set => throw new NotImplementedException(); }
    }
    public class MockLayer : ILayer
    {
        public MockLayer(int id)
        {
            Id = id;
            if (id == 3) { N = 8; }
            else { N = 4; }
        }

        public int Id { get; set; }
        public int N { get; set; }
        public IMatrix Input { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IMatrix Output { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IMatrix Weights { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IMatrix Biases { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ActivationFunction ActivationFunction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILayer ReceptiveField { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILayer ProjectiveField { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string LoggableName => throw new NotImplementedException();
        public void ProcessInput(IMatrix originalInput = null)
        {
            throw new NotImplementedException();
        }
        public string ToLog()
        {
            throw new NotImplementedException();
        }
    }
    public class MockNetParameters : INetParameters
    {
        public string FileName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ObservableCollection<ILayerParameters> LayerParametersCollection
        {
            get => new ObservableCollection<ILayerParameters>
                {
                    new MockLayerParameters(1), new MockLayerParameters(2), new MockLayerParameters(3), new MockLayerParameters(4), new MockLayerParameters(5)
                };
            set => throw new NotImplementedException();
        }
        public WeightInitType WeightInitType { get => WeightInitType.Xavier; set => throw new NotImplementedException(); }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class MockLayerParameters : ILayerParameters
    {
        public MockLayerParameters(int id)
        {
            Id = id;
            WeightMin = -1;
            WeightMax = 1;
            BiasMin = -1;
            BiasMax = 1;

            if (id == 0)
            {
                NeuronsPerLayer = 4;
                ActivationType = ActivationType.NullActivator;
            }
            else if (id == 3)
            {
                NeuronsPerLayer = 8;
                ActivationType = ActivationType.LeakyReLU;
            }
            else
            {
                NeuronsPerLayer = 4;
                ActivationType = ActivationType.Tanh;
            }
        }

        public int Id { get; set; }
        public int NeuronsPerLayer { get; set; }
        public float WeightMin { get; set; }
        public float WeightMax { get; set; }
        public float BiasMin { get; set; }
        public float BiasMax { get; set; }
        public ActivationType ActivationType { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class MockTrainer : ITrainer
    {
        public INet OriginalNet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILearningNet LearningNet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public INet TrainedNet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISampleSet SampleSet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SamplesTotal { get => 1000; set => throw new NotImplementedException(); }
        public int Epochs { get => 10; set => throw new NotImplementedException(); }
        public int CurrentEpoch { get => 3; set => throw new NotImplementedException(); }
        public int CurrentSample { get => 742; set => throw new NotImplementedException(); }
        public float LearningRate { get => .1f; set => throw new NotImplementedException(); }
        public float LearningRateChange { get => .9f; set => throw new NotImplementedException(); }
        public float LastEpochsAccuracy { get => .625f; set => throw new NotImplementedException(); }
        public float CurrentTotalCost { get => .00037647f; set => throw new NotImplementedException(); }
        public TrainerStatus TrainerStatus { get => TrainerStatus.Running; set => throw new NotImplementedException(); }
        public string Message { get => "Training..."; set => throw new NotImplementedException(); }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public Task Reset()
        {
            throw new NotImplementedException();
        }
        public Task TestAsync(Sample[] testingSamples, ILogger logger = null)
        {
            throw new NotImplementedException();
        }
        public Task Train(string logName, int epochs)
        {
            throw new NotImplementedException();
        }
        public bool IsInitialized { get => true; set => throw new NotImplementedException(); }
    }
    public class MockTrainerParameters : ITrainerParameters
    {
        public int Epochs { get => 10; set => throw new NotImplementedException(); }
        public float LearningRate { get => .1f; set => throw new NotImplementedException(); }
        public float LearningRateChange { get => .9f; set => throw new NotImplementedException(); }
        public CostType CostType { get => CostType.SquaredMeanError; set => throw new NotImplementedException(); }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class MockSampleSet : ISampleSet
    {
        public SampleSetParameters Parameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Sample[] TestingSamples { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Sample[] TrainingSamples { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
    public class MockSampleSetSteward : ISampleSetSteward
    {
        public ISampleSet SampleSet => throw new NotImplementedException();
        public Dictionary<SetName, ISampleSetParameters> DefaultSampleSetParameters => throw new NotImplementedException();
        public IEnumerable<SampleType> Types => throw new NotImplementedException();
        public Task<ISampleSet> CreateSampleSetAsync(ISampleSetParameters sampleSetParameters) => throw new NotImplementedException();
        public Task<ISampleSet> CreateDefaultSampleSetAsync(SetName setName) => throw new NotImplementedException();
        public string Message => "SampleSet created.";
        public PropertyChangedEventHandler[] EventHandlers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
