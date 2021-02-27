using AIDemoUI.Commands;
using AIDemoUI.Views;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Autofac.Features.AttributeFilters;

namespace AIDemoUI.ViewModels
{
    public interface IStartStopVM : IBaseVM
    {
        bool IsLogged { get; set; }
        bool IsPaused { get; set; }
        bool IsStarted { get; set; }
        bool IsFinished { get; }
        string LogName { get; set; }
        string StepButtonText { get; set; }
        string TrainButtonText { get; set; }

        IAsyncCommand InitializeNetCommand { get; set; }
        IAsyncCommand ShowSampleImportWindowCommand { get; set; }
        IAsyncCommand TrainCommand { get; set; }
        Task InitializeNetAsync(object parameter);
        Task ShowSampleImportWindow(object parameter);
        Task TrainAsync(object parameter);
        bool TrainAsync_CanExecute(object parameter);
        void Trainer_PropertyChanged(object sender, PropertyChangedEventArgs e);
    }

    public class StartStopVM : BaseSubVM, IStartStopVM
    {
        #region fields & ctor

        private readonly ISessionContext _sessionContext;
        private bool isLogged;
        private string logName, stepButtonText, trainButtonText;
        private readonly SampleImportWindow _sampleImportWindow;
        private readonly PropertyChangedEventHandler _trainer_PropertyChanged_inLayerParametersVM, _trainer_PropertyChanged_inStatusVM;

        public StartStopVM(ISessionContext sessionContext, ISimpleMediator mediator, SampleImportWindow sampleImportWindow, 
            [KeyFilter("InStatusVM")] PropertyChangedEventHandler trainer_propertyChanged_inStatusVM,
            [KeyFilter("InLayerParametersVM")] PropertyChangedEventHandler trainer_propertyChanged_inLayerParametersVM)
            : base(mediator)
        {
            _sessionContext = sessionContext;
            _sampleImportWindow = sampleImportWindow;
            _trainer_PropertyChanged_inLayerParametersVM = trainer_propertyChanged_inLayerParametersVM;  // vgl mediator
            _trainer_PropertyChanged_inStatusVM = trainer_propertyChanged_inStatusVM;

            _mediator.Register("Token: MainWindowVM", StartStopVMCallback);

            TrainButtonText = "Train";
            StepButtonText = "Step";
        }
        private void StartStopVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region properties

        private INetParameters NetParameters => _sessionContext.NetParameters;
        private ITrainerParameters TrainerParameters => _sessionContext.TrainerParameters;
        private INet Net
        {
            get => _sessionContext.Net;
            set => _sessionContext.Net = value;
        }
        private ITrainer Trainer
        {
            get => _sessionContext.Trainer;
            set => _sessionContext.Trainer = value;
        }
        private SampleSet SampleSet => _sessionContext.SampleSet;
        private bool IsSampleSetInitialized => _sessionContext.IsSampleSetInitialized;
        private bool IsNetInitialized
        {
            get => _sessionContext.IsNetInitialized;
            set => _sessionContext.IsNetInitialized = value;
        }
        private bool IsTrainerInitialized
        {
            get => _sessionContext.IsTrainerInitialized;
            set => _sessionContext.IsTrainerInitialized = value;
        }

        public bool IsStarted
        {
            get
            {
                return Trainer == null ? false : Trainer.IsStarted;
            }
            set
            {
                if (Trainer.IsStarted != value)
                {
                    Trainer.IsStarted = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TrainButtonText)); // Call in Base.OnPropChgd?
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
                }
            }
        }
        public bool IsFinished => Trainer.IsFinished;
        public string TrainButtonText
        {
            get
            {
                return trainButtonText;
            }
            set
            {
                if (trainButtonText != value)
                {
                    trainButtonText = value;
                    OnPropertyChanged();
                }
            }
        }
        public string StepButtonText
        {
            get
            {
                return stepButtonText;
            }
            set
            {
                if (stepButtonText != value)
                {
                    stepButtonText = value;
                    OnPropertyChanged();
                }
            }
        }
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

        #endregion

        #region Commands

        public IAsyncCommand InitializeNetCommand { get; set; }
        public IAsyncCommand ShowSampleImportWindowCommand { get; set; }
        public IAsyncCommand TrainCommand { get; set; }

        #region Executes and CanExecutes

        public async Task ShowSampleImportWindow(object parameter)// Only use async commands??
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _sampleImportWindow.Show(); // use a delegate?
                });
            });
        }
        public async Task InitializeNetAsync(object parameter)
        {
            if (!IsNetInitialized)
            {
                Net = await Task.Run(() => Initializer.InitializeNet(Net, NetParameters));  // as ext meth (in NetBuilderFactory)?
                IsNetInitialized = true;
            }

            if (!IsTrainerInitialized)
            {
                Trainer = await Task.Run(() => Initializer.InitializeTrainer(Trainer, Net.GetCopy(), TrainerParameters, SampleSet));                
                Trainer.PropertyChanged += _trainer_PropertyChanged_inLayerParametersVM;
                Trainer.PropertyChanged += _trainer_PropertyChanged_inStatusVM;
                Trainer.PropertyChanged += Trainer_PropertyChanged;
                IsTrainerInitialized = true;
            }
        }
        [DebuggerStepThrough]
        public bool InitializeNetAsync_CanExecute(object parameter)
        {
            return IsSampleSetInitialized;
        }
        public async Task TrainAsync(object parameter)
        {
            bool isStepModeOn = (bool)parameter;

            if (IsStarted)
            {
                if (IsPaused)
                {
                    if (!isStepModeOn) IsPaused = false;
                    await Trainer.Train(IsLogged ? LogName : string.Empty, _sessionContext.TrainerParameters.Epochs);
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
                await Trainer.Train(IsLogged ? LogName : string.Empty, _sessionContext.TrainerParameters.Epochs);
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
            return IsSampleSetInitialized && IsNetInitialized && IsTrainerInitialized;
        }

        #endregion

        #endregion

        #region events

        public void Trainer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #endregion
    }
}
