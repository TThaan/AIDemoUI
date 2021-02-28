using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.RawData;
using static AIDemoUI.SampleData.MockData;

namespace AIDemoUI.SampleDataViewModel
{
    public class MainWindowVMSampleData : MainWindowVM
    {
        #region ctor

        public MainWindowVMSampleData()
            : base(MockSessionContext, SampleNetParametersVM, SampleStartStopVM, SampleStatusVM, SampleMediator)
        {
        }

        #endregion
    }
}
