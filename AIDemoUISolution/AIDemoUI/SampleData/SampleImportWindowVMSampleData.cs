using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.SampleDataInitializer;

namespace AIDemoUI.SampleData
{
    public class SampleImportWindowVMSampleData : SampleImportWindowVM
    {
        #region ctor

        public SampleImportWindowVMSampleData()
            : base(SampleMediator, SampleSamplesSteward) { }

        #endregion
    }
}
