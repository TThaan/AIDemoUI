using AIDemoUI.Commands;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using NNet_InputProvider;
using System;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public class MainWindowVM : BaseVM
    {
        #region ctor & fields

        INetParameters _netParameters;
        ITrainerParameters _trainerParameters;
        SampleSetParameters _sampleSetParameters;
        int observerGap, progressBarValue, progressBarMax;
        string progressBarText;
        bool paused, started, stepwise;
        IAsyncCommand runCommandAsync;
        IRelayCommand stepCommand;

        public MainWindowVM()
        {
            _netParameters = new NetParameters();
            _trainerParameters = new TrainerParameters();
            NetParametersVM = new NetParametersVM(_netParameters, _trainerParameters);
            // NetParametersVM.OkBtnPressed += OnOkButtonPressedAsync;

            SetDefaultValues();
        }

        #region helpers

        void SetDefaultValues()
        {
            ObserverGap = 100;
            ProgressBarValue = 0;
            ProgressBarMax = 1;
            ProgressBarText = "Wpf AI Demo";
            Started = false;
            Paused = true;
        }

        #endregion

        #endregion

        #region public

        public NetParametersVM NetParametersVM { get; }
        /// <summary>
        /// How many samples in training are skipped until the next ui-notification event.
        /// </summary>
        public int ObserverGap
        {
            get
            {
                return observerGap;
            }
            set
            {
                if (observerGap != value)
                {
                    observerGap = value;
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
                }
            }
        }
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
        public string RunButtonText => Started ? "Cancel" : "Run";
        public string StepButtonText => Paused ? "Continue" : "Pause";
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
        async Task RunCommandAsync_Execute(object parameter)
        {
            // Use dedicated class (main/initializer/Start):
            SampleImportVM vm = NetParametersVM.SampleImportWindow.DataContext as SampleImportVM;
            _sampleSetParameters = vm.SelectedTemplate;

            await Task.Run(async () =>
            {
                Paused = false;

                SampleSet sampleSet = await GetSampleSetAsync();
                Initializer initializer = await GetInitializerAsync();

                ProgressBarValue = 0;
                ProgressBarMax = _trainerParameters.Epochs * (_sampleSetParameters.TestingSamples + _sampleSetParameters.TrainingSamples);

                ObserverGap = (int)Math.Round((decimal)(_sampleSetParameters.TrainingSamples + _sampleSetParameters.TestingSamples), 0);

                if (Stepwise)
                {
                    // initializer.Trainer.Paused += Trainer_Paused;
                    await initializer.Trainer.Train(sampleSet.TrainingSamples, sampleSet.TestingSamples, 0);
                }
                else
                {
                    try
                    {
                        await initializer.Trainer.Train(sampleSet.TrainingSamples, sampleSet.TestingSamples, ObserverGap);
                    }
                    catch (Exception e)
                    {
                        // throw;
                    }
                }
            });
        }
        bool RunCommandAsync_CanExecute(object parameter)
        {
            SampleImportVM vm = NetParametersVM.SampleImportWindow.DataContext as SampleImportVM;

            if (vm != null &&
                string.IsNullOrEmpty(vm.Url_TrainingLabels) ||
                string.IsNullOrEmpty(vm.Url_TrainingImages) ||
                string.IsNullOrEmpty(vm.Url_TestingLabels) ||
                string.IsNullOrEmpty(vm.Url_TestingImages))
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
        void StepCommand_Execute(object parameter)
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
        bool StepCommand_CanExecute(object parameter)
        {
            SampleImportVM vm = NetParametersVM.SampleImportWindow.DataContext as SampleImportVM;

            if (vm != null &&
                string.IsNullOrEmpty(vm.Url_TrainingLabels) ||
                string.IsNullOrEmpty(vm.Url_TrainingImages) ||
                string.IsNullOrEmpty(vm.Url_TestingLabels) ||
                string.IsNullOrEmpty(vm.Url_TestingImages))
            {
                return false;
            }
            return true;
        }

        #region helpers


        async Task<SampleSet> GetSampleSetAsync()
        {
            SampleSet result = Creator.GetSampleSet(_sampleSetParameters);
            await result.SetSamples();
            return result;  // Task..
        }
        async Task<Initializer> GetInitializerAsync()
        {
            return await Task.Run(() =>
            {
                // Check if layer ids match their position in array.

                Initializer result = new Initializer();
                NetParametersVM.SynchronizeModelToVM();
                result.Net = result.GetNet(_netParameters);
                result.Trainer = result.GetTrainer(result.Net, _trainerParameters);
                // Initializer.Trainer.SomethingHappend += Trainer_SomethingHappend;

                return result;
            });
        }

        #endregion

        #endregion

        #region Events Handling Methods (OK Button, Trainer)

        Task Trainer_Paused(string stepFinished)
        {
            Paused = true;
            ProgressBarText = stepFinished;
            while (Paused == true)
            {

            }
            return Task.FromResult(0);
        }
        void Trainer_SomethingHappend(string whatHappend)
        {
            foreach (var layerVM in NetParametersVM.LayerParameterVMs)
            {
                //layerVM.OnLayerUpdate();
            }
            ProgressBarValue += ObserverGap;
            ProgressBarText = whatHappend;
            //Thread.Sleep(100);
        }
        void SampleCreator_SomethingHappend(string whatHappend)
        {
            ProgressBarText = whatHappend;
            //Thread.Sleep(100);
        }

        #endregion
    }
}
