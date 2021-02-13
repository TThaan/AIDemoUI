using NeuralNetBuilder;
using System.ComponentModel;
using System.Threading;

namespace AIDemoUI.ViewModels
{
    public interface IStatusVM
    {
        int CurrentEpoch { get; set; }
        int CurrentSample { get; set; }
        float CurrentTotalCost { get; set; }
        int Epochs { get; set; }
        float LastEpochsAccuracy { get; set; }
        int ProgressBarMax { get; set; }
        string ProgressBarText { get; set; }
        int ProgressBarValue { get; set; }
    }

    public class StatusVM : BaseSubVM//, IStatusVM
    {
        #region fields & ctor

        int epochs, currentEpoch, currentSample, progressBarValue, progressBarMax;
        float lastEpochsAccuracy, currentTotalCost;
        string progressBarText;
        private readonly ISessionContext _sessionContext;

        public StatusVM(ISessionContext sessionContext)
        {
            SetDefaultValues();
            _sessionContext = sessionContext;
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

        public int Epochs
        {
            get
            {
                return epochs;
            }
            set
            {
                if (epochs != value)
                {
                    epochs = value;
                    OnPropertyChanged();
                }
            }
        }
        public int CurrentEpoch
        {
            get
            {
                return currentEpoch;
            }
            set
            {
                if (currentEpoch != value)
                {
                    currentEpoch = value;
                    OnPropertyChanged();
                }
            }
        }
        public int CurrentSample
        {
            get
            {
                return currentSample;
            }
            set
            {
                if (currentSample != value)
                {
                    currentSample = value;
                    OnPropertyChanged();
                }
            }
        }
        public float CurrentTotalCost
        {
            get
            {
                return currentTotalCost;
            }
            set
            {
                if (currentTotalCost != value)
                {
                    currentTotalCost = value;
                    OnPropertyChanged();
                }
            }
        }
        public float LastEpochsAccuracy
        {
            get
            {
                return lastEpochsAccuracy;
            }
            set
            {
                if (lastEpochsAccuracy != value)
                {
                    lastEpochsAccuracy = value;
                    OnPropertyChanged();
                }
            }
        }
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
        //public void StartStopVM_SubViewModelChanged(object s, SubViewModelChangedEventArgs e)
        //{

        //}
        //public void NetParametersVM_SubViewModelChanged(object s, SubViewModelChangedEventArgs e)
        //{
        //    // Compute ProgressBarValue in percent!
        //}

        #endregion
    }
}
