using AIDemoUI.Commands;
using CustomLogger;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public class StartStopVM : BaseSubVM
    {
        #region fields & ctor
        
        bool paused, started, stepwise, isLogged;
        IAsyncCommand runCommandAsync;
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
        }

        #endregion

        #endregion

        #region public

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
                    OnPropertyChanged(nameof(RunButtonText));
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
        public string RunButtonText => Started ? "Cancel" : "Run";
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

        #region helpers (debugging only)

        static void Serialize<T>(T target, string fileName)
        {
            using (Stream stream = File.Open(fileName, FileMode.Create))// + ".dat"
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, target);
            }
        }
        static T DeSerialize<T>(string name)
        {
            using (Stream stream = File.Open(name, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (T)bf.Deserialize(stream);
            }
        }

        #endregion

        #endregion

        #region Commands

        public IAsyncCommand RunCommandAsync
        {
            get
            {
                if (runCommandAsync == null)
                {
                    runCommandAsync = new AsyncRelayCommand(RunCommandAsync_Execute, RunCommandAsync_CanExecute);
                }
                return runCommandAsync;
            }
        }
        // No Relay here..
        private async Task RunCommandAsync_Execute(object parameter)
        {
            Paused = false;

            // Create and dispose of Logger in Trainer? Just pass bool 'IsLogged' and path?
            using (ILogger logger = IsLogged
                ? new CustomLogger.Logger(Path.GetTempPath() + "AIDemoUI.txt")  // Declare 'string path'.
                : null)
            {
                SampleSet sampleSet = await GetSampleSetAsync();    // ISampleSet ?
                IInitializer initializer = await GetInitializerAsync(logger);
                initializer.Trainer.StatusChanged += _mainVM.StatusVM.Trainer_StatusChanged;

                if (Stepwise)
                {
                    // initializer.Trainer.Paused += Trainer_Paused;
                    await initializer.Trainer.Train(sampleSet.TrainingSamples, sampleSet.TestingSamples);
                }
                else
                {
                    try
                    {
                        // sampleSet.TrainingSamples = DeSerialize<Sample[]>(@"C:\Users\Jan_PC\Documents\_NeuralNetApp\TrainingData.dat");
                        // sampleSet.TestingSamples = DeSerialize<Sample[]>(@"C:\Users\Jan_PC\Documents\_NeuralNetApp\TestingData.dat");
                        await initializer.Trainer.Train(sampleSet.TrainingSamples, sampleSet.TestingSamples);
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }
            }
        }
        private bool RunCommandAsync_CanExecute(object parameter)
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

        #region helpers

        private async Task<SampleSet> GetSampleSetAsync()
        {
            SampleSet result = Creator.GetSampleSet((_mainVM.SampleImportWindow.DataContext as SampleImportWindowVM).SelectedTemplate);
            result.StatusChanged += _mainVM.StatusVM.SampleSet_StatusChanged;
            await result.SetSamples();  // use DI container
            return result;  // Task..            
        }
        private async Task<IInitializer> GetInitializerAsync(ILogger logger)
        {
            return await Task.Run(() => new Initializer(
                _mainVM.NetParametersVM.NetParameters, _mainVM.NetParametersVM.TrainerParameters, logger));
        }

        #endregion

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
