using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.MockData;
using static AIDemoUI.SampleData.RawData;

namespace AIDemoUI.SampleDataViewModelViewModel
{
    public class SampleImportWindowVMSampleData : SampleImportWindowVM
    {
        #region ctor

        public SampleImportWindowVMSampleData()
            : base(MockSessionContext, SampleMediator, SampleSamplesSteward) { }

        #endregion
    }
}
