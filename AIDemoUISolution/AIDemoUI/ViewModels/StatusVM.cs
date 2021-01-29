using NeuralNetBuilder;
using System.Threading;

namespace AIDemoUI.ViewModels
{
    public class StatusVM : BaseSubVM
    {
        #region fields & ctor

        int progressBarValue, progressBarMax;
        string progressBarText;

        public StatusVM(MainWindowVM mainVM)
            : base(mainVM)
        {
            SetDefaultValues();
            //_mainVM.Trainer.StatusChanged += 
        }

        #region helpers

        void SetDefaultValues()
        {
            ProgressBarValue = 0;
            ProgressBarMax = 100;
            ProgressBarText = "Wpf AI Demo";
        }

        #endregion

        #endregion

        #region public

        public int ProgressBarMax
        {
            get
            {
                return progressBarMax;
            }
            set
            {
                if (progressBarMax != value)
                {
                    progressBarMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public int ProgressBarValue
        {
            get
            {
                return progressBarValue;
            }
            set
            {
                if (progressBarValue != value)
                {
                    progressBarValue = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ProgressBarText
        {
            get
            {
                return progressBarText;
            }
            set
            {
                if (progressBarText != value)
                {
                    progressBarText = value;
                    OnPropertyChanged();
                }
            }
        }
        public void StartStopVM_SubViewModelChanged(object s, SubViewModelChangedEventArgs e)
        {

        }
        public void NetParametersVM_SubViewModelChanged(object s, SubViewModelChangedEventArgs e)
        {
            // Compute ProgressBarValue in percent!
        }

        #endregion

        #region events

        // Consider general (common) EventsLib or use (eg) INPC.
        public void Anything_StatusChanged(object s, StatusChangedEventArgs e)
        {

        }
        public void SampleSet_StatusChanged(object sender, DeepLearningDataProvider.StatusChangedEventArgs e)
        {
            _mainVM.StatusVM.ProgressBarText = e.Info;
            Thread.Sleep(200);
        }
        public void Trainer_StatusChanged(object sender, NeuralNetBuilder.StatusChangedEventArgs e)
        {
            ITrainer trainer = sender as ITrainer;

            _mainVM.StatusVM.ProgressBarText = e.Info;
            if (trainer != null)
            {
                _mainVM.StatusVM.ProgressBarValue = trainer.CurrentEpoch;
            }
        }

        #endregion
    }
}
