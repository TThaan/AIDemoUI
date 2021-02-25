using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.SampleDataInitializer;

namespace AIDemoUI.SampleData
{
    public class MainWindowVMSampleData : MainWindowVM
    {
        #region ctor

        public MainWindowVMSampleData()
            : base(SampleSessionContext, SampleNetParametersVM, SampleStartStopVM, SampleStatusVM, SampleMediator)
        {
        }

        #endregion
    }
}
