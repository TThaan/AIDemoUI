using AIDemoUI.Views;
using DeepLearningDataProvider;

namespace AIDemoUI.ViewModels
{
    public class MainWindowVM : BaseVM
    {
        #region ctor & fields

        public MainWindowVM()
        {
            NetParametersVM = new NetParametersVM(this);
            StartStopVM = new StartStopVM(this);
            StatusVM = new StatusVM(this);
            SamplesImportVM = new SamplesImportVM(this);

            SampleImportWindow = new SampleImportWindow();  // ...s..

            NetParametersVM.SubViewModelChanged += StatusVM.NetParametersVM_SubViewModelChanged;    // Consider a central SubViewModelChanged handling method in MainVM?
            StartStopVM.SubViewModelChanged += StatusVM.StartStopVM_SubViewModelChanged;            // Consider a central SubViewModelChanged handling method in MainVM?
        }

        #endregion

        #region public

        public NetParametersVM NetParametersVM { get; set; }
        public StatusVM StatusVM { get; set; }
        public StartStopVM StartStopVM { get; set; }
        public SamplesImportVM SamplesImportVM { get; set; }
        public SampleImportWindow SampleImportWindow { get; set; }
        public SampleSetParameters SampleSetParameters { get; set; }

        #endregion
    }
}
