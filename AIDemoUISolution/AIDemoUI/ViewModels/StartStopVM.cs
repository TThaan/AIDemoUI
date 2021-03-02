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
        bool IsSampleSetInitialized { get; }
        bool IsNetInitialized { get; }
        bool IsTrainerInitialized { get; }
        bool IsPaused { get; set; }
        bool IsStarted { get; set; }
        bool IsFinished { get; }
        bool IsLogged { get; set; }
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
        }
        private void StartStopVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region properties (No Commands)

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
        private INetParameters NetParameters => _sessionContext.NetParameters;
        private ITrainerParameters TrainerParameters => _sessionContext.TrainerParameters;
        private ISampleSet SampleSet => _sessionContext.SampleSet;

        public bool IsSampleSetInitialized => _sessionContext.SampleSet != null;
        public bool IsNetInitialized => _sessionContext.Net.IsInitialized;
        public bool IsTrainerInitialized => _sessionContext.Trainer.IsInitialized;
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
        public string TrainButtonText => GetTrainButtonText();
        public string StepButtonText => GetStepButtonText();

        #region helpers

        private string GetTrainButtonText()
        {
            if(IsStarted && !IsPaused)
            {
                return "Pause";
            }
            else if(IsStarted && IsPaused)
            {
                return "Continue";
            }
            else { return "Train"; }
        }
        private string GetStepButtonText()
        {

            return "Step";
        }

        #endregion

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
                    _sampleImportWindow.ShowDialog(); // use a delegate?
                });
            });
        }
        public async Task InitializeNetAsync(object parameter)
        {
            if (!IsNetInitialized)
            {
                Net = await Task.Run(() => Initializer.InitializeNet(Net, NetParameters));  // as ext meth (in NetBuilderFactory)?
            }

            if (!IsTrainerInitialized)
            {
                Trainer = await Task.Run(() => Initializer.InitializeTrainer(Trainer, Net.GetCopy(), TrainerParameters, SampleSet)); 
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
