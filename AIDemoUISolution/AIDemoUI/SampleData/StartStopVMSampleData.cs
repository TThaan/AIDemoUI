using AIDemoUI.ViewModels;

namespace AIDemoUI.SampleData
{
    public class StartStopVMSampleData : StartStopVM
    {
        public StartStopVMSampleData()
            : base(SampleDataBootStrapper.SampleSessionContext)
        {
        }
    }
}
