using AIDemoUI.ViewModels;

namespace AIDemoUI.SampleData
{
    public class MainWindowVMSampleData : MainWindowVM
    {
        #region ctor

        public MainWindowVMSampleData()
        {
            ProgressBarMax = 10000;
            ProgressBarValue = 3800;
            ProgressBarText = $"Current Accuracy: 0.625";
        }

        #endregion
    }
}
