using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI
{
    public interface ISessionContext
    {
        INet Net { get; set; }
        ITrainer Trainer { get; set; }
        ISampleSet SampleSet { get; set; }
        INetParameters NetParameters { get; set; }
        ITrainerParameters TrainerParameters { get; set; }
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

        #region properties

        public INet Net { get; set; }
        public ITrainer Trainer { get; set; }
        public ISampleSet SampleSet { get; set; }
        public INetParameters NetParameters { get; set; }
        public ITrainerParameters TrainerParameters { get; set; }

        #endregion
    }
}
