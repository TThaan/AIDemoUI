using AIDemoUI.Commands;
using AIDemoUI.Views;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public interface IStartStopVM
    {
        INet Net { get; set; }
        ITrainer Trainer { get; set; }
        SampleSet SampleSet { get; set; }
        bool IsLogged { get; set; }
        bool IsPaused { get; set; }
        bool IsStarted { get; set; }
        string LogName { get; set; }
        string StepButtonText { get; }
        string TrainButtonText { get; }

        IAsyncCommand InitializeNetCommand { get; set; }
        IRelayCommand ShowSampleImportWindowCommand { get; set; }
        IAsyncCommand TrainCommand { get; set; }
        Task InitializeNetAsync(object parameter);
        void ShowSampleImportWindow(object parameter);
        Task TrainAsync(object parameter);
        bool TrainAsync_CanExecute(object parameter);
    }

    public class StartStopVM : BaseSubVM, IStartStopVM
    {
        #region fields & ctor

        INet net;
        SampleSet sampleSet;
        ITrainer trainer;
        bool isStarted, isLogged;
        string logName;
        private readonly SampleImportWindow _sampleImportWindow;
        private readonly INetParameters _netParameters;
        private readonly ITrainerParameters _trainerParameters;

        public StartStopVM(ISimpleMediator mediator, SampleImportWindow sampleImportWindow, INetParameters netParameters, ITrainerParameters trainerParameters)
            : base(mediator)
        {
            _mediator.Register("Token: MainWindowVM", StartStopVMCallback);
            _sampleImportWindow = sampleImportWindow;
            _netParameters = netParameters;
            _trainerParameters = trainerParameters;
        }
        private void StartStopVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

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

        public IAsyncCommand InitializeNetCommand { get; set; }
        public IRelayCommand ShowSampleImportWindowCommand { get; set; }
        public IAsyncCommand TrainCommand { get; set; }

        #region Executes and CanExecutes

        public async Task InitializeNetAsync(object parameter)
        {
            Net = await Task.Run(() => Initializer.GetNet(_netParameters));
        }
        public void ShowSampleImportWindow(object parameter)
        {
            _sampleImportWindow.Show(); // use a delegate?
        }
        public async Task TrainAsync(object parameter)
        {
            bool isStepModeOn = (bool)parameter;

            if (Trainer == null)
            {
                Trainer = await Task.Run(() => Initializer.GetTrainer(Net.GetCopy(), _trainerParameters, SampleSet));
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
        public bool TrainAsync_CanExecute(object parameter)
        {
            return SampleSet != null && Net != null;
        }

        #endregion

        #endregion
    }
}
