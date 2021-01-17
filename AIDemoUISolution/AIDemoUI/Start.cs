using AIDemoUI.ViewModels;
using NeuralNetBuilder;
using NNet_InputProvider;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using static AIDemoUI.ViewModels.NetParametersVM;

namespace AIDemoUI
{
    public class Start
    {
        #region fields

        static MainWindowVM _mainVM;

        #endregion

        #region public

        internal static async Task Run(MainWindowVM mainVM)
        {
            _mainVM = mainVM ?? throw new NullReferenceException($"{nameof(Run)}.ctor: " +
                    $"Parameter {nameof(mainVM)}.");

            //
            SampleImportVM vm = _mainVM.NetParametersVM.SampleImportWindow.DataContext as SampleImportVM;
            _mainVM.SampleSetParameters = vm.SelectedTemplate;

            await Task.Run(async () =>
            {
                _mainVM.Paused = false;

                // ISampleSet ?
                SampleSet sampleSet = await GetSampleSetAsync();
                Initializer initializer = await GetInitializerAsync();
                initializer.Trainer.StatusChanged += Trainer_StatusChanged;
                _mainVM.ProgressBarMax = _mainVM.TrainerParameters.Epochs - 1;// * (_mainVM.SampleSetParameters.TestingSamples + _mainVM.SampleSetParameters.TrainingSamples)
                _mainVM.ProgressBarValue = 0;

                _mainVM.ObserverGap = (int)Math.Round((decimal)(_mainVM.SampleSetParameters.TrainingSamples + _mainVM.SampleSetParameters.TestingSamples), 0);

                if (_mainVM.Stepwise)
                {
                    // initializer.Trainer.Paused += Trainer_Paused;
                    await initializer.Trainer.Train(sampleSet.TrainingSamples, sampleSet.TestingSamples, 0);
                }
                else
                {
                    try
                    {
                        sampleSet.TrainingSamples = DeSerialize<Sample[]>(@"C:\Users\Jan_PC\Documents\_NeuralNetApp\TrainingData.dat");
                        sampleSet.TestingSamples = DeSerialize<Sample[]>(@"C:\Users\Jan_PC\Documents\_NeuralNetApp\TestingData.dat");
                        await initializer.Trainer.Train(sampleSet.TrainingSamples, sampleSet.TestingSamples, _mainVM.ObserverGap);
                    }
                    catch (Exception e)
                    {
                        // throw;
                    }
                }
            });
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

        #region helpers

        static async Task<SampleSet> GetSampleSetAsync()
        {
            SampleSet result = Creator.GetSampleSet(_mainVM.SampleSetParameters);
            result.StatusChanged += SampleSet_StatusChanged;
            await result.SetSamples();
            return result;  // Task..
        }
        static async Task<Initializer> GetInitializerAsync()
        {
            return await Task.Run(() =>
            {
                // Check if layer ids match their position in array.

                Initializer result = new Initializer();

                switch (_mainVM.NetParametersVM.Mode)
                {
                    case RunningMode.Create:
                        _mainVM.NetParametersVM.SynchronizeModelToVM();
                        result.Net = result.GetNet(_mainVM.NetParameters);
                        break;
                    case RunningMode.CreateAndSave:
                        _mainVM.NetParametersVM.SynchronizeModelToVM();
                        result.Net = result.GetNet(_mainVM.NetParameters, new FileInfo(_mainVM.NetParametersVM.FileName));
                        break;
                    case RunningMode.Load:
                        result.Net = result.GetNet(new FileInfo(_mainVM.NetParametersVM.FileName));
                        break;
                }

                result.Trainer = result.GetTrainer(result.Net, _mainVM.TrainerParameters);
                // Initializer.Trainer.SomethingHappend += Trainer_SomethingHappend;

                return result;
            });
        }

        #endregion

        #region events

        private static void SampleSet_StatusChanged(object sender, NNet_InputProvider.StatusChangedEventArgs e)
        {
            _mainVM.ProgressBarText = e.Info;
            Thread.Sleep(200);
        }
        private static void Trainer_StatusChanged(object sender, NeuralNetBuilder.StatusChangedEventArgs e)
        {
            ITrainer trainer = sender as ITrainer;

            _mainVM.ProgressBarText = e.Info;
            if (trainer != null)
            {
                _mainVM.ProgressBarValue = trainer.CurrentEpoch;
            }
        }

        #endregion
    }
}
