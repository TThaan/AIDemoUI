using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI
{
    public interface ISessionContext
    {
        INetParameters NetParameters { get; set; }
        ITrainerParameters TrainerParameters { get; set; }
        SampleSet SampleSet { get; set; }
        INet Net { get; set; }
        ITrainer Trainer { get; set; }
        bool IsNetInitialized { get; set; }         // in INet?
        bool IsSampleSetInitialized { get; set; }   // in SampleSet?
        bool IsTrainerInitialized { get; set; }     // in ITrainer?
    }

    /// <summary>
    /// Contains (model) data shared among different view models.
    /// </summary>
    public class SessionContext : ISessionContext
    {
        #region ctor

        public SessionContext(INetParameters netParameters, ITrainerParameters trainerParameters, INet net, ITrainer trainer)   // Also inject RawSampleSet?? No, it's runtime only..
        {
            NetParameters = netParameters;
            TrainerParameters = trainerParameters;
            Net = net;
            Trainer = trainer;
        }

        #endregion

        public INetParameters NetParameters { get; set; }
        public ITrainerParameters TrainerParameters { get; set; }
        public INet Net { get; set; }
        public ITrainer Trainer { get; set; }
        public SampleSet SampleSet { get; set; }
        public bool IsNetInitialized { get; set; }
        public bool IsSampleSetInitialized { get; set; }
        public bool IsTrainerInitialized { get; set; }
    }
}
