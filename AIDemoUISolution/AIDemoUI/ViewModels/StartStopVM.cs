using AIDemoUI.Commands;
using AIDemoUI.Views;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace AIDemoUI.ViewModels
{
    public interface IStartStopVM : IBaseVM
    {
        INet Net { get; set; }
        ITrainer Trainer { get; set; }
        SampleSet SampleSet { get; }
        bool IsLogged { get; set; }
        bool IsPaused { get; set; }
        bool IsStarted { get; set; }
        string LogName { get; set; }
        string StepButtonText { get; }
        string TrainButtonText { get; }

        IAsyncCommand InitializeNetCommand { get; set; }
        IAsyncCommand ShowSampleImportWindowCommand { get; set; }
        IAsyncCommand TrainCommand { get; set; }
        Task InitializeNetAsync(object parameter);
        Task ShowSampleImportWindow(object parameter);
        Task TrainAsync(object parameter);
        bool TrainAsync_CanExecute(object parameter);
    }

    public class StartStopVM : BaseSubVM, IStartStopVM
    {
        #region fields, private properties & ctor

        private readonly ISessionContext _sessionContext;
        INetParameters _netParameters => _sessionContext.NetParameters;
        ITrainerParameters _trainerParameters => _sessionContext.TrainerParameters;
        INet net;//sessionContext?
        ITrainer trainer;//sessionContext?
        bool isStarted, isLogged;
        string logName;
        private readonly SampleImportWindow _sampleImportWindow;

        public StartStopVM(ISessionContext sessionContext, ISimpleMediator mediator, SampleImportWindow sampleImportWindow)
            : base(mediator)
        {
            _sessionContext = sessionContext;

            _sampleImportWindow = sampleImportWindow;
            _mediator.Register("Token: MainWindowVM", StartStopVMCallback);
        }
        private void StartStopVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region public

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
        public SampleSet SampleSet => _sessionContext.SampleSet;
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
        public IAsyncCommand ShowSampleImportWindowCommand { get; set; }
        public IAsyncCommand TrainCommand { get; set; }

        #region Executes and CanExecutes

        public async Task InitializeNetAsync(object parameter)
        {
            Net = await Task.Run(() => Initializer.GetNet(_netParameters));
        }
        public async Task ShowSampleImportWindow(object parameter)// Only use async commands??
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _sampleImportWindow.Show(); // use a delegate?
                });
            });
            OnPropertyChanged(nameof(SampleSet));
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
        [DebuggerStepThrough]
        public bool TrainAsync_CanExecute(object parameter)
        {
            OnPropertyChanged(nameof(TrainButtonText));
            return SampleSet != null && Net != null;
        }

        #endregion

        #endregion
    }
}
