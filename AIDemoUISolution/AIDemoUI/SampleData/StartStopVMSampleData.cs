using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.SampleDataBootStrapper;

namespace AIDemoUI.SampleData
{
    public class StartStopVMSampleData : StartStopVM
    {
        public StartStopVMSampleData()
            : base(SampleSessionContext, null, SampleSampleImportWindow)
        {
        }
    }
}
