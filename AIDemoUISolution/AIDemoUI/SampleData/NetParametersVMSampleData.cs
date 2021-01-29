using AIDemoUI.ViewModels;

namespace AIDemoUI.SampleData
{
    public class NetParametersVMSampleData : NetParametersVM
    {
        #region ctor

        /// <summary>
        /// Ctor used by NetParametersVMSampleData
        /// </summary>
        public NetParametersVMSampleData() : base(new MainWindowVM()) { }
        /// <summary>
        /// Ctor used by MainVMSampleData
        /// </summary>
        public NetParametersVMSampleData(MainWindowVM mainVM) : base(mainVM) { }

        #endregion
    }
}
