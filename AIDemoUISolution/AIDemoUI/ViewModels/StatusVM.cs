using NeuralNetBuilder;
using System.ComponentModel;
using System.Threading;

namespace AIDemoUI.ViewModels
{
    public class StatusVM : BaseSubVM
    {
        #region fields & ctor

        int epochs, currentEpoch, currentSample, progressBarValue, progressBarMax;
        float lastEpochsAccuracy, currentTotalCost;
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
            ProgressBarText = e.Info;
            Thread.Sleep(200);
        }
        public void Trainer_StatusChanged(object sender, NeuralNetBuilder.StatusChangedEventArgs e)
        {
            ProgressBarText = e.Info;
        }
        public void Trainer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ITrainer trainer = sender as ITrainer;

            if (trainer != null)
            {
                switch (e.PropertyName)
                {
                    case nameof(trainer.CurrentSample):
                        CurrentSample = trainer.CurrentSample;
                        break;
                    case nameof(trainer.CurrentTotalCost):
                        CurrentTotalCost = trainer.CurrentTotalCost;
                        break;
                    case nameof(trainer.LastEpochsAccuracy):
                        LastEpochsAccuracy = trainer.LastEpochsAccuracy;
                        ProgressBarText = $"Training...\n(Last Epoch's Accuracy: {trainer.LastEpochsAccuracy})";
                        break;
                    case nameof(trainer.CurrentEpoch):
                        CurrentEpoch = trainer.CurrentEpoch;
                        ProgressBarValue = CurrentEpoch;
                        break;
                    case nameof(trainer.LearningRate):
                        break;
                    case nameof(trainer.Epochs):
                        Epochs = trainer.Epochs;
                        ProgressBarMax = Epochs;
                        break;
                    case nameof(trainer.IsStarted):
                        _mainVM.StatusVM.ProgressBarMax = trainer.Epochs;
                        _mainVM.StatusVM.ProgressBarText = $"Training...\nLast Epoch's Accuracy: {LastEpochsAccuracy}";
                        break;
                    case nameof(trainer.IsPaused):
                        _mainVM.StatusVM.ProgressBarText = $"Training paused...\nLast Epoch's Accuracy: {LastEpochsAccuracy}";
                        break;
                    case nameof(trainer.IsFinished):
                        _mainVM.StatusVM.ProgressBarText = $"Training finished.\nLast Epoch's Accuracy: {LastEpochsAccuracy}";
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
