using System;

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

    public class StatusVM : BaseSubVM, IStatusVM
    {
        #region fields & ctor

        int epochs, currentEpoch, currentSample, progressBarValue, progressBarMax;
        float lastEpochsAccuracy, currentTotalCost;
        string progressBarText;

        public StatusVM(ISimpleMediator mediator)
            : base(mediator)
        {
            _mediator.Register("Token: MainWindowVM", StatusVMCallback);
            //_mainVM.Trainer.StatusChanged += 
        }

        private void StatusVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

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

        #endregion
    }
}
