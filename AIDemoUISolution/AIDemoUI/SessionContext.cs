using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI
{
    public interface ISessionContext
    {
        INetParameters NetParameters { get; set; }
        ITrainerParameters TrainerParameters { get; set; }
        ISampleSet SampleSet { get; set; }
        ISampleSetSteward SampleSetSteward { get; }
        INet Net { get; set; }
        ITrainer Trainer { get; set; }
    }

    /// <summary>
    /// Contains (model) data shared among different view models.
    /// </summary>
    public class SessionContext : ISessionContext
    {
        #region ctor

        public SessionContext(INetParameters netParameters, ITrainerParameters trainerParameters, INet net, ITrainer trainer, ISampleSetSteward sampleSetSteward)   // Also inject RawSampleSet?? No, it's runtime only..
        {
            NetParameters = netParameters;
            TrainerParameters = trainerParameters;
            Net = net;
            Trainer = trainer;
            SampleSetSteward = sampleSetSteward;
        }

        #endregion

        #region properties

        public INetParameters NetParameters { get; set; }
        public ITrainerParameters TrainerParameters { get; set; }
        public INet Net { get; set; }
        public ITrainer Trainer { get; set; }
        public ISampleSetSteward SampleSetSteward { get; }
        public ISampleSet SampleSet { get; set; }

        #endregion
    }
}
