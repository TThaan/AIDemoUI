using AIDemoUI.ViewModels;

namespace AIDemoUI.SampleData
{
    public class StatusVMSampleData : StatusVM
    {
        /// <summary>
        /// Ctor used by StatusVMSampleData
        /// </summary>
        public StatusVMSampleData()
            : base(new MainWindowVM())
        {
            ProgressBarMax = 100;
            ProgressBarValue = 38;
            ProgressBarText = $"Training...\n(Last Epoch's Accuracy: 0.625)";
        }
        /// <summary>
        /// Ctor used by MainVMSampleData
        /// </summary>
        public StatusVMSampleData(MainWindowVM mainVM)
            : base(mainVM)
        {
            ProgressBarMax = 100;
            ProgressBarValue = 38;
            ProgressBarText = $"Training...\n(Last Epoch's Accuracy: 0.625)";
        }
    }
}
