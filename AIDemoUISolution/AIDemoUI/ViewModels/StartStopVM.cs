using AIDemoUI.Commands.Async;
using AIDemoUI.Views;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

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

        IAsyncRelayCommand InitializeNetCommand { get; }
        IAsyncRelayCommand ShowSampleImportWindowCommand { get; }
        IAsyncRelayCommand TrainCommand { get; }
        IAsyncRelayCommand StepCommand { get; }
    }

    public class StartStopVM : BaseVM, IStartStopVM
    {
        #region fields & ctor

        private bool isLogged;
        private string logName;
        private readonly SampleImportWindow _sampleImportWindow;

        public StartStopVM(ISessionContext sessionContext, ISimpleMediator mediator, 
            SampleImportWindow sampleImportWindow)
            : base(sessionContext, mediator)
        {
            _sampleImportWindow = sampleImportWindow;

            RegisterMediatorHandlers();
            DefineCommands();
        }

        #region helpers

        private void RegisterMediatorHandlers()
        { 
            _mediator.Register(MediatorToken.StartStopVM_UpdateButtonTexts.ToString(), UpdateButtonTexts);
        }
        private void DefineCommands()
        {
            InitializeNetCommand = new AsyncRelayCommand(InitializeNetAsync, InitializeNetAsync_CanExecute);
            ShowSampleImportWindowCommand = new SimpleAsyncRelayCommand(ShowSampleImportWindow);
            TrainCommand = new AsyncRelayCommand(TrainAsync, TrainAsync_CanExecute)
            { IsConcurrentExecutionAllowed = true };
            StepCommand = new AsyncRelayCommand(StepAsync, StepAsync_CanExecute);
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

        public IAsyncRelayCommand InitializeNetCommand { get; private set; }
        public IAsyncRelayCommand ShowSampleImportWindowCommand { get; private set; }
        public IAsyncRelayCommand TrainCommand { get; private set; }
        public IAsyncRelayCommand StepCommand { get; private set; }

        #region Executes and CanExecutes

        private async Task ShowSampleImportWindow(object parameter)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _sampleImportWindow.ShowDialog(); // use delegate?
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
            OnPropertyChanged(nameof(InitializeNetButtonText));
        }
        [DebuggerStepThrough]
        private bool InitializeNetAsync_CanExecute(object parameter)
        {
            return SampleSet != null;
        }
        private async Task TrainAsync(object parameter)
        {
            if (Trainer.TrainerStatus == TrainerStatus.Running)
                Trainer.TrainerStatus = TrainerStatus.Paused;
            else Trainer.TrainerStatus = TrainerStatus.Running;
                
            await Trainer.Train(IsLogged ? LogName : string.Empty, _sessionContext.TrainerParameters.Epochs);
            Net = Trainer.TrainedNet.GetCopy();

            if (Trainer.TrainerStatus == TrainerStatus.Finished)
                await Trainer.Reset();
        }
        [DebuggerStepThrough]
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
        [DebuggerStepThrough]
        private bool StepAsync_CanExecute(object parameter)
        {
            return SampleSet != null
                && Net.NetStatus != NetStatus.Undefined 
                && Trainer.TrainerStatus != TrainerStatus.Undefined;
        }

        #endregion

        #endregion

        #region mediator handlers

        private void UpdateButtonTexts(object obj)
        {
            OnPropertyChanged(nameof(ImportSamplesButtonText));
            OnPropertyChanged(nameof(InitializeNetButtonText));
            OnPropertyChanged(nameof(TrainButtonText));
            OnPropertyChanged(nameof(StepButtonText));
        }

        #endregion
    }
}
