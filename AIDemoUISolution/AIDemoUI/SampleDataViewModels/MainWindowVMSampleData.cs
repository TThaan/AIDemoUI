using AIDemoUI.ViewModels;
using AIDemoUI.SampleData.MockData;
using static AIDemoUI.SampleData.RawData;

namespace AIDemoUI.SampleDataViewModels
{
    public class MainWindowVMSampleData : MainWindowVM
    {
        #region ctor

        public MainWindowVMSampleData()
            : base(new MockSessionContext(), RawNetParametersVM, RawStartStopVM, RawStatusVM, RawMediator)
        {
        }

        #endregion
    }
}
