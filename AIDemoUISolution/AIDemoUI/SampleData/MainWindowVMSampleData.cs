using AIDemoUI.ViewModels;

namespace AIDemoUI.SampleData
{
    public class MainWindowVMSampleData : MainWindowVM
    {
        #region ctor

        public MainWindowVMSampleData()
            : base(new NetParametersVMSampleData())
        {

        }

        #endregion
    }
}
