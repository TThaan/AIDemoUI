using DeepLearningDataProvider;
using NeuralNetBuilder.FactoriesAndParameters;
using System.Threading.Tasks;

namespace AIDemoUI
{

    public delegate Task OkBtnEventHandler(INetParameters netParameters, bool isTurnBased, SampleSetParameters sampleSetParameters);
}