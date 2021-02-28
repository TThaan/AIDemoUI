using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.MockData;
using static AIDemoUI.SampleData.RawData;

namespace AIDemoUI.SampleDataViewModel
{
    public class StartStopVMSampleData : StartStopVM
    {
        public StartStopVMSampleData()
            : base(MockSessionContext, SampleMediator, SampleSampleImportWindow, null, null)
        {
        }
    }
}
