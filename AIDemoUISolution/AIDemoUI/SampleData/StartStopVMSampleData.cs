using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.SampleDataInitializer;

namespace AIDemoUI.SampleData
{
    public class StartStopVMSampleData : StartStopVM
    {
        public StartStopVMSampleData()
            : base(SampleSessionContext, SampleMediator, SampleSampleImportWindow)
        {
        }
    }
}
