using DeepLearningDataProvider;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI
{
    public interface ISessionContext
    {
        INetParameters NetParameters { get; set; }
        ITrainerParameters TrainerParameters { get; set; }
        SampleSet SampleSet { get; set; }
    }

    /// <summary>
    /// Contains data shared among different view models.
    /// </summary>
    public class SessionContext : ISessionContext
    {
        #region ctor

        public SessionContext(INetParameters netParameters, ITrainerParameters trainerParameters)
        {
            NetParameters = netParameters;
            TrainerParameters = trainerParameters;
        }

        #endregion

        public INetParameters NetParameters { get; set; }
        public ITrainerParameters TrainerParameters { get; set; }
        public SampleSet SampleSet { get; set; }
    }
}
