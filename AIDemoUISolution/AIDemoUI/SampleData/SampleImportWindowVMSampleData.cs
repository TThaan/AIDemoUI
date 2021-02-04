using AIDemoUI.ViewModels;

namespace AIDemoUI.SampleData
{
    public class SampleImportWindowVMSampleData : SampleImportWindowVM
    {
        #region ctor

        /// <summary>
        /// Ctor used by SampleImportWindowVMSampleData
        /// </summary>
        public SampleImportWindowVMSampleData() : base(new MainWindowVM()) { }
        /// <summary>
        /// Ctor used by MainVMSampleData
        /// </summary>
        public SampleImportWindowVMSampleData(MainWindowVM mainVM) : base(mainVM) { }

        #endregion
    }
}
