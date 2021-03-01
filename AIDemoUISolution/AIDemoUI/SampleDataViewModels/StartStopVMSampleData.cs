using AIDemoUI.ViewModels;
using AIDemoUI.SampleData.MockData;
using static AIDemoUI.SampleData.RawData;

namespace AIDemoUI.SampleDataViewModels
{
    public class StartStopVMSampleData : StartStopVM
    {
        public StartStopVMSampleData()
            : base(new MockSessionContext(), RawMediator, RawSampleImportWindow, null, null)
        {
        }
    }
}
