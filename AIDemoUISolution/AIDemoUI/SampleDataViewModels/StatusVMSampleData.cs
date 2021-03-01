using AIDemoUI.ViewModels;
using AIDemoUI.SampleData.MockData;
using static AIDemoUI.SampleData.RawData;

namespace AIDemoUI.SampleDataViewModels
{
    public class StatusVMSampleData : StatusVM
    {
        public StatusVMSampleData()
            : base(new MockSessionContext(), RawMediator)
        {
        }
    }
}
