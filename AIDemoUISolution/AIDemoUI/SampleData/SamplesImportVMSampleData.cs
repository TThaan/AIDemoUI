using AIDemoUI.ViewModels;

namespace AIDemoUI.SampleData
{
    public class SamplesImportVMSampleData : SamplesImportVM
    {
        #region ctor

        /// <summary>
        /// Ctor used by SampleImportVMSampleData
        /// </summary>
        public SamplesImportVMSampleData() : base(new MainWindowVM()) {  }
        /// <summary>
        /// Ctor used by MainVMSampleData
        /// </summary>
        public SamplesImportVMSampleData(MainWindowVM mainVM) : base(mainVM) { }

        #endregion
    }
}
