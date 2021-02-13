using AIDemoUI.ViewModels;

namespace AIDemoUI.SampleData
{
    public class SampleImportWindowVMSampleData : SampleImportWindowVM
    {
        #region ctor

        public SampleImportWindowVMSampleData()
            : base(SampleDataBootStrapper.SampleSessionContext) { }

        #endregion
    }
}
