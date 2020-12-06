using AIDemoUI.Commands;
using FourPixCam;
using NNet_InputProvider;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public class MainWindowVM : BaseVM
    {
        #region ctor & fields

        NetParameters _netParameters;
        int observerGap, progressBarValue, progressBarMax;
        string progressBarText;
        bool paused, started, stepwise;
        IAsyncCommand runCommandAsync;
        IRelayCommand stepCommand;

        public MainWindowVM()
        {
            _netParameters = new NetParameters(WeightInitType.Xavier);
            NetParametersVM = new NetParametersVM(_netParameters);
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
        public Initializer Initializer { get; set; }
        public Trainer Trainer { get; set; }
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
            SampleImportVM vm = NetParametersVM.SampleImportWindow.DataContext as SampleImportVM;
            SampleSetParameters sampleSetParameters = vm.SelectedTemplate;

            SampleSet samples = Creator.GetSampleSet(sampleSetParameters);
            samples.SomethingHappend += SampleCreator_SomethingHappend;
            await samples.SetSamples();

            await Task.Run(async () =>
            {
                Paused = false;

                foreach (var item in NetParametersVM.LayerVMs)
                {
                    if(item.N != item.Layer.N || item.Layer.N != item.Layer.Processed.Input.m)
                    {
                        item.Layer.N = item.N;
                        item.Layer.Processed.Reset();
                    }
                }

                _netParameters.Layers = NetParametersVM.LayerVMs
                    .Select(x => x.Layer)
                    .ToArray();

                Initializer = new Initializer(_netParameters, samples);
                Trainer = Initializer.Trainer;
                Trainer.SomethingHappend += Trainer_SomethingHappend;

                ProgressBarValue = 0;
                ProgressBarMax = _netParameters.EpochCount * (sampleSetParameters.TestingSamples + sampleSetParameters.TrainingSamples);

                ObserverGap = (int)Math.Round((decimal)vm.SelectedTemplate.TrainingSamples / 100, 0);

                if (Stepwise)
                {
                    Trainer.Paused += Trainer_Paused;
                    await Trainer.Train(Initializer.Samples.TrainingSamples, Initializer.Samples.TestingSamples, 0);
                }
                else
                {
                    try
                    {
                        await Trainer.Train(Initializer.Samples.TrainingSamples, Initializer.Samples.TestingSamples, ObserverGap);
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
            if (Paused)
            {
                Paused = false;
                Trainer.Paused -= Trainer_Paused;
            }
            else
            {
                Paused = true;
                Trainer.Paused += Trainer_Paused;
                foreach (var layer in NetParametersVM.LayerVMs)
                {
                    layer.OnLayerUpdate();
                }
                // Thread.Sleep(200);
            }
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
            foreach (var layerVM in NetParametersVM.LayerVMs)
            {
                layerVM.OnLayerUpdate();
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
