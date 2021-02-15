using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.SampleDataBootStrapper;

namespace AIDemoUI.SampleData
{
    public class MainWindowVMSampleData : MainWindowVM
    {
        #region ctor

        public MainWindowVMSampleData()
            : base(SampleNetParametersVM, SampleStartStopVM, SampleStatusVM, SampleSampleImportWindow, SampleLayerParametersVMFactory, null)
        {
            // NetParametersVM = new NetParametersVMSampleData();
            // StatusVM = new StatusVMSampleData();
            // StartStopVM = new StartStopVMSampleData();
        }

        #endregion
    }
}
