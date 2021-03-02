using AIDemoUI.ViewModels;
using AIDemoUI.SampleData.MockData;
using static AIDemoUI.SampleData.RawData;

namespace AIDemoUI.SampleDataViewModels
{
    public class SampleImportWindowVMSampleData : SampleImportWindowVM
    {
        #region ctor

        public SampleImportWindowVMSampleData()
            : base(new MockSessionContext(), RawMediator, new MockSampleSetSteward())
        {
            //throw new System.ArgumentException($"{TrainingSamples}");
        }

        #endregion
    }
}
