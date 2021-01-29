using AIDemoUI.ViewModels;
using System;

namespace AIDemoUI.SampleData
{
    public class MainWindowVMSampleData : MainWindowVM
    {
        #region ctor

        public MainWindowVMSampleData()
        {
            NetParametersVM = new NetParametersVMSampleData(this);
            StatusVM = new StatusVMSampleData(this);
            StartStopVM = new StartStopVMSampleData(this);
        }

        #endregion
    }
}
