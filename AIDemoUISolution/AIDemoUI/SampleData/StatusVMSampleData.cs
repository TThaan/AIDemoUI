using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.SampleDataBootStrapper;

namespace AIDemoUI.SampleData
{
    public class StatusVMSampleData : StatusVM
    {
        public StatusVMSampleData()
            : base(SampleSessionContext, SampleMediator)
        {
            ProgressBarMax = 100;
            ProgressBarValue = 38;
            ProgressBarText = $"Training...\n(Last Epoch's Accuracy: 0.625)";
        }
    }
}
