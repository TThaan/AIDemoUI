using AIDemoUI.Commands.Async;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Threading.Tasks;
using System.Windows;
using AIDemoUI.FactoriesAndStewards;

namespace AIDemoUI.ViewModels
{
    public interface IStartStopVM : IBaseVM
    {
        bool IsLogged { get; set; }
        string LogName { get; set; }
        string ImportSamplesButtonText { get; }
        string InitializeNetButtonText { get; }
        string StepButtonText { get; }
        string TrainButtonText { get; }

        IAsyncCommand ShowSampleImportWindowCommand { get; }
        IAsyncRaisableCommand InitializeNetCommand { get; }
        IAsyncRaisableCommand TrainCommand { get; }
        IAsyncRaisableCommand StepCommand { get; }
        void Trainer_TrainerStatusChanged(object sender, TrainerStatusChangedEventArgs e);
    }

    public class StartStopVM : BaseVM, IStartStopVM
    {
        #region fields & ctor

        private bool isLogged;
        private string logName;
        private readonly Func<bool?> _showSampleImportWindow;

        public StartStopVM(ISessionContext sessionContext, ISimpleMediator mediator, 
            IDelegateFactory delegateFactory)
            : base(sessionContext, mediator)
        {
            _showSampleImportWindow = delegateFactory.ShowSampleImportWindow();

            RegisterMediatorHandlers();
            DefineCommands();
        }

        #region helpers

        private void RegisterMediatorHandlers()
        { 
            _mediator.Register(MediatorToken.StartStopVM_OnSampleSetInitialized.ToString(), OnSampleSetInitialized);
        }
        private void DefineCommands()
        {
            ShowSampleImportWindowCommand = new SimpleAsyncRelayCommand(ShowSampleImportWindow);
            InitializeNetCommand = new AsyncRelayCommand_Raisable(InitializeNetAsync, InitializeNetAsync_CanExecute);
            TrainCommand = new AsyncRelayCommand_Raisable(TrainAsync, TrainAsync_CanExecute)
            { IsConcurrentExecutionAllowed = true };
            StepCommand = new AsyncRelayCommand_Raisable(StepAsync, StepAsync_CanExecute);
        }

        #endregion

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
        private ISampleSet SampleSet => _sessionContext.SampleSet;
        private INetParameters NetParameters => _sessionContext.NetParameters;
        private ITrainerParameters TrainerParameters => _sessionContext.TrainerParameters;

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
        public string ImportSamplesButtonText => GetImportSamplesButtonText();
        public string InitializeNetButtonText => GetInitializeNetButtonText();
        public string TrainButtonText => GetTrainButtonText();
        public string StepButtonText => GetStepButtonText();

        #region helpers

        private string GetImportSamplesButtonText()
        {
            if(SampleSet == null)
                return "Import Samples (UnDone)";
            else return "Import Samples (Done)"; 
        }
        private string GetInitializeNetButtonText()
        {
            if (Net.NetStatus == NetStatus.Initialized)
                return "Initialize Net (Done)";
            else return "Initialize Net (Undone)";
        }
        private string GetTrainButtonText()
        {
            if (Trainer.TrainerStatus == TrainerStatus.Running)
                return "Pause";
            else if (Trainer.TrainerStatus == TrainerStatus.Paused)
                return "Continue";
            else return "Train";
        }
        private string GetStepButtonText()
        {
            return "Step";
        }

        #endregion

        #endregion

        #region Commands

        public IAsyncCommand ShowSampleImportWindowCommand { get; private set; }
        public IAsyncRaisableCommand InitializeNetCommand { get; private set; }
        public IAsyncRaisableCommand TrainCommand { get; private set; }
        public IAsyncRaisableCommand StepCommand { get; private set; }

        #region Executes and CanExecutes

        private async Task ShowSampleImportWindow(object parameter)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _showSampleImportWindow();
                });
            });
        }
        private async Task InitializeNetAsync(object parameter)
        {
            if (Net.NetStatus == NetStatus.Undefined)
            {
                Net = await Task.Run(() => Initializer.InitializeNet(Net, NetParameters));
            }

            if (Trainer.TrainerStatus == TrainerStatus.Undefined)
            {
                Trainer = await Task.Run(() => Initializer.InitializeTrainer(Trainer, Net.GetCopy(), TrainerParameters, SampleSet)); 
            }

            OnNetInitialized();
        }
        private bool InitializeNetAsync_CanExecute(object parameter)
        {
            return SampleSet != null;
        }
        private async Task TrainAsync(object parameter)
        {
            if (Trainer.TrainerStatus == TrainerStatus.Running)
            {
                Trainer.TrainerStatus = TrainerStatus.Paused;
                return;
            }

            await Trainer.Train(IsLogged ? LogName : string.Empty, _sessionContext.TrainerParameters.Epochs);
            Net = Trainer.TrainedNet.GetCopy();

            if (Trainer.TrainerStatus == TrainerStatus.Finished)
            { 
                await Trainer.Reset();
            }
        }
        private bool TrainAsync_CanExecute(object parameter)
        {
            return SampleSet != null 
                && Net.NetStatus != NetStatus.Undefined 
                && Trainer.TrainerStatus != TrainerStatus.Undefined;
        }
        private async Task StepAsync(object parameter)
        {
            Trainer.TrainerStatus = TrainerStatus.Paused;
            await Trainer.Train(IsLogged ? LogName : string.Empty, _sessionContext.TrainerParameters.Epochs);
            Net = Trainer.TrainedNet.GetCopy();

            if (Trainer.TrainerStatus == TrainerStatus.Finished)
                await Trainer.Reset();
        }
        private bool StepAsync_CanExecute(object parameter)
        {
            return SampleSet != null
                && Net.NetStatus != NetStatus.Undefined 
                && Trainer.TrainerStatus != TrainerStatus.Undefined;
        }

        #endregion

        #endregion

        #region mediator handlers

        private void OnSampleSetInitialized(object obj)
        {
            OnPropertyChanged(nameof(ImportSamplesButtonText));
            OnPropertyChanged(nameof(InitializeNetButtonText));
            OnPropertyChanged(nameof(TrainButtonText));
            OnPropertyChanged(nameof(StepButtonText));

            InitializeNetCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region helpers

        public void Trainer_TrainerStatusChanged(object sender, TrainerStatusChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TrainButtonText));
            Application.Current.Dispatcher.Invoke(() =>
            {
                TrainCommand.RaiseCanExecuteChanged();
                StepCommand.RaiseCanExecuteChanged();
            });
        }
        private void OnNetInitialized()
        {
            OnPropertyChanged(nameof(InitializeNetButtonText));
            TrainCommand.RaiseCanExecuteChanged();
            StepCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
