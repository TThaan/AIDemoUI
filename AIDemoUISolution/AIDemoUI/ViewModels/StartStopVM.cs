using AIDemoUI.Commands;
using AIDemoUI.Views;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public class StartStopVM : BaseSubVM
    {
        #region fields & ctor

        INet net;
        SampleSet sampleSet;
        ITrainer trainer;
        bool isStarted, isLogged;
        string logName;
        IAsyncCommand initializeNetCommandAsync, trainCommandAsync;
        IRelayCommand importSamplesCommand;
        private readonly SampleImportWindow _sampleImportWindow;

        public StartStopVM(ISessionContext sessionContext, SimpleMediator mediator, SampleImportWindow sampleImportWindow)
            : base(sessionContext, mediator)
        {
            _mediator.Register("Token: MainWindowVM", StartStopVMCallback);
            _sampleImportWindow = sampleImportWindow;
            SetDefaultValues();
        }

        private void StartStopVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

        #region helpers

        void SetDefaultValues()
        {
            IsStarted = false;
            IsLogged = false;
            LogName = Path.GetTempPath() + "AIDemoUI.txt";
        }

        #endregion

        #endregion

        #region public

        public SampleSet SampleSet
        {
            get
            {
                return sampleSet;
            }
            set
            {
                if (sampleSet != value)
                {
                    sampleSet = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TrainButtonText));
                    OnSubViewModelChanged();
                }
            }
        }
        public INet Net
        {
            get
            {
                return net;
            }
            set
            {
                if (net != value)
                {
                    net = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TrainButtonText));
                    OnSubViewModelChanged();
                }
            }
        }
        public ITrainer Trainer
        {
            get
            {
                return trainer;
            }
            set
            {
                if (trainer != value)
                {
                    trainer = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TrainButtonText));
                    OnSubViewModelChanged();
                }
            }
        }
        public bool IsStarted
        {
            get
            {
                return isStarted;
            }
            set
            {
                if (isStarted != value)
                {
                    isStarted = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TrainButtonText)); // Call in Base.OnPropChgd?
                    OnSubViewModelChanged();
                }
            }
        }
        public bool IsPaused
        {
            get
            {
                return Trainer == null ? true : Trainer.IsPaused;
            }
            set
            {
                if (Trainer.IsPaused != value)
                {
                    Trainer.IsPaused = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TrainButtonText));
                    OnSubViewModelChanged();
                }
            }
        }
        public string TrainButtonText => GetTrainButtonText();
        public string StepButtonText => "Step";
        public bool IsLogged
        {
            get
            {
                return isLogged;
            }
            set
            {
                if (isLogged != value)
                {
                    isLogged = value;
                    OnPropertyChanged();
                }
            }
        }
        public string LogName
        {
            get
            {
                return logName;
            }
            set
            {
                if (logName != value)
                {
                    logName = value;
                    OnPropertyChanged();
                }
            }
        }

        #region helpers

        private string GetTrainButtonText()
        {
            if (Net == null && SampleSet == null)
            {
                return "You need to initialize the net and\nimport a sample set to start training.";
            }
            else if (Net == null && SampleSet != null)
            {
                return "You need to initialize the net\nto start training.";
            }
            else if (Net != null && SampleSet == null)
            {
                return "You need to import a sample set\nto start training.";
            }
            else
            {
                if (IsStarted && IsPaused)
                {
                    return "Continue";
                }
                else if (!IsStarted && IsPaused)
                {
                    return "Train";
                }
                else if (IsStarted & !IsPaused)
                {
                    return "Pause";
                }
                else
                {
                    return "Error: The Trainer cannot be unstarted\nand unpaused at the same time.";
                }
            }
        }

        #endregion

        #endregion

        #region Commands

        public IAsyncCommand InitializeNetCommandAsync
        {
            get
            {
                if (initializeNetCommandAsync == null)
                {
                    initializeNetCommandAsync = new AsyncRelayCommand(InitializeNetCommandAsync_Execute, x => true);
                }
                return initializeNetCommandAsync;
            }
        }
        private async Task InitializeNetCommandAsync_Execute(object parameter)
        {
            Net = await Task.Run(() => Initializer.GetNet(_sessionContext.NetParameters));
        }
        public IRelayCommand ImportSamplesCommand
        {
            get
            {
                if (importSamplesCommand == null)
                {
                    importSamplesCommand = new RelayCommand(ImportSamplesCommand_Execute, ImportSamplesCommand_CanExecute);
                }
                return importSamplesCommand;
            }
        }
        void ImportSamplesCommand_Execute(object parameter)
        {
            _sampleImportWindow.Show();
        }
        bool ImportSamplesCommand_CanExecute(object parameter)
        {
            return true;
        }
        public IAsyncCommand TrainCommandAsync
        {
            get
            {
                if (trainCommandAsync == null)
                {
                    trainCommandAsync = new AsyncRelayCommand(TrainCommandAsync_Execute, TrainCommandAsync_CanExecute);
                }
                return trainCommandAsync;
            }
        }

        private async Task TrainCommandAsync_Execute(object parameter)  // Trainer = DIC-injected?
        {
            bool isStepModeOn = (bool)parameter;

            if (Trainer == null)
            {
                Trainer = await Task.Run(() => Initializer.GetTrainer(Net.GetCopy(), _sessionContext.TrainerParameters, SampleSet));
                // Trainer.PropertyChanged += _sessionContext.MainWindowVM.Trainer_PropertyChanged;
            }

            if (IsStarted)
            {
                if (IsPaused)
                {
                    if (!isStepModeOn) IsPaused = false;
                    await Trainer.Train(IsLogged ? LogName : string.Empty);
                }
                else
                {
                    IsPaused = true;
                }
            }
            else
            {
                IsStarted = true;
                if (!isStepModeOn) IsPaused = false;
                await Trainer.Train(IsLogged ? LogName : string.Empty);
                Net = Trainer.TrainedNet.GetCopy();

                if (Trainer.IsFinished)
                {
                    await Trainer.Reset();
                    IsPaused = true;
                    IsStarted = false;
                }
            }
        }
        private bool TrainCommandAsync_CanExecute(object parameter)
        {
            return SampleSet != null && Net != null;
        }

        #endregion
    }
}
