using AIDemoUI.ViewModels;

namespace AIDemoUI.SampleData
{
    public class StatusVMSampleData : StatusVM
    {
        public StatusVMSampleData()
            : base(SampleDataBootStrapper.SampleSessionContext)
        {
            ProgressBarMax = 100;
            ProgressBarValue = 38;
            ProgressBarText = $"Training...\n(Last Epoch's Accuracy: 0.625)";
        }
    }
}
