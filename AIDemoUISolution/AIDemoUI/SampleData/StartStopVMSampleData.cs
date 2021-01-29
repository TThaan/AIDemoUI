using AIDemoUI.ViewModels;

namespace AIDemoUI.SampleData
{
    public class StartStopVMSampleData : StartStopVM
    {
        /// <summary>
        /// Ctor used by StartStopVMSampleData
        /// </summary>
        public StartStopVMSampleData()
            : base(new MainWindowVM())
        {
            Paused = true;
            Stepwise = true;
        }
        /// <summary>
        /// Ctor used by MainVMSampleData
        /// </summary>
        public StartStopVMSampleData(MainWindowVM mainVM)
            : base(mainVM)
        {
            Paused = true;
            Stepwise = true;
        }
    }
}
