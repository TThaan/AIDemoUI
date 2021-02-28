using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.MockData;
using static AIDemoUI.SampleData.RawData;

namespace AIDemoUI.SampleDataViewModel
{
    public class StatusVMSampleData : StatusVM
    {
        public StatusVMSampleData()
            : base(MockSessionContext, SampleMediator)
        {
        }
    }
}
