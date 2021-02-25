using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.SampleDataInitializer;

namespace AIDemoUI.SampleData
{
    public class StatusVMSampleData : StatusVM
    {
        public StatusVMSampleData()
            : base(SampleMediator)
        {
            ProgressBarMax = 100;
            ProgressBarValue = 38;
            ProgressBarText = $"Training...\n(Last Epoch's Accuracy: 0.625)";
            CurrentEpoch = 3;
            CurrentSample = 742;
            CurrentTotalCost = .00037647f;
        }
    }
}
