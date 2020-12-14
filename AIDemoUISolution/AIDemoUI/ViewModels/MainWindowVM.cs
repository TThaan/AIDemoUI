using AIDemoUI.Commands;
using FourPixCam;
using MatrixHelper;
using NNet_InputProvider;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        // public Trainer Trainer { get; set; }
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

            await Task.Run(async () =>
            {
                Paused = false;

                // await UseUIGeneratedNetAndSamples(sampleSetParameters);
                // UseExample01();
                UseExample02();

                ProgressBarValue = 0;
                ProgressBarMax = _netParameters.EpochCount * (Initializer.Samples.Parameters.TestingSamples + Initializer.Samples.Parameters.TrainingSamples);

                ObserverGap = (int)Math.Round((decimal)(Initializer.Samples.Parameters.TrainingSamples + Initializer.Samples.Parameters.TestingSamples), 0);

                if (Stepwise)
                {
                    Initializer.Trainer.Paused += Trainer_Paused;
                    await Initializer.Trainer.Train(Initializer.Samples.TrainingSamples, Initializer.Samples.TestingSamples, 0);
                }
                else
                {
                    try
                    {
                        await Initializer.Trainer.Train(Initializer.Samples.TrainingSamples, Initializer.Samples.TestingSamples, ObserverGap);
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
                Initializer.Trainer.Paused -= Trainer_Paused;
            }
            else
            {
                Paused = true;
                Initializer.Trainer.Paused += Trainer_Paused;
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

        #region helpers


        // Better: Factories separated from FourPixCam (at least: (incl) Net -> as lib)
        // Initializer as static class! (In Trainig Lib?)
        async Task UseUIGeneratedNetAndSamples(SampleSetParameters sampleSetParameters)
        {
            Initializer = new Initializer();

            Initializer.Samples = await SampleSetFactory.GetSampleSet(NetParametersVM, _netParameters, sampleSetParameters);
            Initializer.Net = NeuralNetFactory.GetNeuralNet(Initializer, NetParametersVM, _netParameters);

            Initializer.Trainer = new Trainer(Initializer.Net, _netParameters);
            Initializer.Trainer.SomethingHappend += Trainer_SomethingHappend;
        }
        void UseExample01()
        {
            Initializer = new Initializer();

            Initializer.Samples = SampleSetFactory.GetSampleSet01();
            Initializer.Net = NeuralNetFactory.GetNeuralNet_Example01(NetParametersVM, _netParameters);

            Initializer.Trainer = new Trainer(Initializer.Net, _netParameters);
            Initializer.Trainer.SomethingHappend += Trainer_SomethingHappend;
        }
        void UseExample02()
        {
            Initializer = new Initializer();

            Initializer.Samples = SampleSetFactory.GetSampleSet02();
            Initializer.Net = NeuralNetFactory.GetNeuralNet(Initializer, NetParametersVM, _netParameters);

            using (Stream stream = File.Open(@"C:\Users\Jan_PC\Documents\_NeuralNetApp\" + "Weights.dat", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Matrix[] Weights = (Matrix[])bf.Deserialize(stream);

                for (int i = 1; i < Initializer.Net.Layers.Length; i++)
                {
                    Initializer.Net.Layers[i].Weights = Weights[i];
                }
            }

            Initializer.Trainer = new Trainer(Initializer.Net, _netParameters);
            Initializer.Trainer.SomethingHappend += Trainer_SomethingHappend;
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
