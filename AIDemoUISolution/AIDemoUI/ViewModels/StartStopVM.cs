using AIDemoUI.Commands;
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
        
        bool paused, started, stepwise, isLogged;
        string logName;
        IAsyncCommand getNetCommandAsync, getTrainerCommandAsync, getSamplesCommandAsync, trainCommandAsync;
        IRelayCommand stepCommand;
        
        public StartStopVM(MainWindowVM mainVM)
            : base(mainVM)
        {
            SetDefaultValues();
        }

        #region helpers

        void SetDefaultValues()
        {
            Started = false;
            Paused = true;
            IsLogged = false;
            Stepwise = false;
            LogName = Path.GetTempPath() + "AIDemoUI.txt";
        }

        #endregion

        #endregion

        #region public

        public SampleSet SampleSet { get; set; }    // ISampleSet
        public INet Net { get; set; }
        public ITrainer Trainer { get; set; }
        public bool Started
        {
            get
            {
                return started;
            }
            set
            {
                if (started != value)
                {
                    started = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TrainButtonText));
                    OnSubViewModelChanged();
                }
            }
        }
        public bool Paused
        {
            get
            {
                return paused;
            }
            set
            {
                if (paused != value)
                {
                    paused = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StepButtonText));
                    OnSubViewModelChanged();
                }
            }
        }
        public string TrainButtonText => Started ? "Cancel" : "Run";
        public string StepButtonText => Paused ? "Continue" : "Pause";
        public bool Stepwise
        {
            get
            {
                return stepwise;
            }
            set
            {
                if (stepwise != value)
                {
                    stepwise = value;
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

        public IAsyncCommand GetNetCommandAsync
        {
            get
            {
                if (getNetCommandAsync == null)
                {
                    getNetCommandAsync = new AsyncRelayCommand(GetNetCommandAsync_Execute, x => true);
                }
                return getNetCommandAsync;
            }
        }
        private async Task GetNetCommandAsync_Execute(object parameter)
        {
            Net = await Task.Run(() => Initializer.GetNet(_mainVM.NetParametersVM.NetParameters));
        }
        public IAsyncCommand GetTrainerCommandAsync
        {
            get
            {
                if (getTrainerCommandAsync == null)
                {
                    getTrainerCommandAsync = new AsyncRelayCommand(GetTrainerCommandAsync_Execute, x => true);
                }
                return getTrainerCommandAsync;
            }
        }
        private async Task GetTrainerCommandAsync_Execute(object parameter)
        {
            Trainer = await Task.Run(() => Initializer.GetTrainer(Net, _mainVM.NetParametersVM.TrainerParameters)); // Pass Net copy?
            Trainer.StatusChanged += _mainVM.StatusVM.Trainer_StatusChanged;    // DIC
        }
        public IAsyncCommand GetSamplesCommandAsync
        {
            get
            {
                if (getSamplesCommandAsync == null)
                {
                    getSamplesCommandAsync = new AsyncRelayCommand(GetSamplesCommandAsync_Execute, x => true);
                }
                return getSamplesCommandAsync;
            }
        }
        private async Task GetSamplesCommandAsync_Execute(object parameter)
        {
            SampleSet = Creator.GetSampleSet((_mainVM.SampleImportWindow.DataContext as SampleImportWindowVM).SelectedTemplate);
            SampleSet.StatusChanged += _mainVM.StatusVM.SampleSet_StatusChanged;
            await SampleSet.SetSamples();  // DIC
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
        // No Relay here..
        private async Task TrainCommandAsync_Execute(object parameter)
        {
            Paused = false;

            if (Stepwise)
            {
                if (!IsLogged) LogName = default;
                await Trainer.Train(SampleSet.TrainingSamples, SampleSet.TestingSamples, LogName);
            }
            else
            {
                try
                {
                    await Trainer.Train(SampleSet.TrainingSamples, SampleSet.TestingSamples);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        private bool TrainCommandAsync_CanExecute(object parameter)
        {
            SampleImportWindowVM vm = _mainVM.SampleImportWindow.DataContext as SampleImportWindowVM;

            if (vm != null &&
                (string.IsNullOrEmpty(vm.Url_TrainingLabels) ||
                string.IsNullOrEmpty(vm.Url_TrainingImages) ||
                string.IsNullOrEmpty(vm.Url_TestingLabels) ||
                string.IsNullOrEmpty(vm.Url_TestingImages)))
            {
                return false;
            }

            return true;
        }
        public IRelayCommand StepCommand
        {
            get
            {
                if (stepCommand == null)
                {
                    stepCommand = new RelayCommand(StepCommand_Execute, StepCommand_CanExecute);
                }
                return stepCommand;
            }
        }
        private void StepCommand_Execute(object parameter)
        {
            //if (Paused)
            //{
            //    Paused = false;
            //    Initializer.Trainer.Paused -= Trainer_Paused;
            //}
            //else
            //{
            //    Paused = true;
            //    Initializer.Trainer.Paused += Trainer_Paused;
            //    foreach (var layer in NetParametersVM.LayerVMs)
            //    {
            //        layer.OnLayerUpdate();
            //    }
            //    // Thread.Sleep(200);
            //}
        }
        private bool StepCommand_CanExecute(object parameter)
        {
            //SampleImportVM vm = NetParametersVM.SampleImportWindow.DataContext as SampleImportVM;

            //if (vm != null &&
            //    string.IsNullOrEmpty(vm.Url_TrainingLabels) ||
            //    string.IsNullOrEmpty(vm.Url_TrainingImages) ||
            //    string.IsNullOrEmpty(vm.Url_TestingLabels) ||
            //    string.IsNullOrEmpty(vm.Url_TestingImages))
            //{
            //    return false;
            //}
            return true;
        }

        #endregion

        #region Events Handling Methods (OK Button, Trainer)

        Task Trainer_Paused(string stepFinished)
        {
            Paused = true;
            _mainVM.StatusVM.ProgressBarText = stepFinished;
            while (Paused == true)
            {

            }
            return Task.FromResult(0);
        }

        #endregion
    }
}
