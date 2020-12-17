using AIDemoUI.ViewModels;
using NeuralNetBuilder;
using NNet_InputProvider;
using System;
using System.IO;
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

            SampleImportVM vm = _mainVM.NetParametersVM.SampleImportWindow.DataContext as SampleImportVM;
            mainVM.SampleSetParameters = vm.SelectedTemplate;

            await Task.Run(async () =>
            {
                _mainVM.Paused = false;

                SampleSet sampleSet = await GetSampleSetAsync();
                Initializer initializer = await GetInitializerAsync();

                _mainVM.ProgressBarValue = 0;
                _mainVM.ProgressBarMax = _mainVM.TrainerParameters.Epochs * (_mainVM.SampleSetParameters.TestingSamples + _mainVM.SampleSetParameters.TrainingSamples);

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
                        await initializer.Trainer.Train(sampleSet.TrainingSamples, sampleSet.TestingSamples, _mainVM.ObserverGap);
                    }
                    catch (Exception e)
                    {
                        // throw;
                    }
                }
            });
        }

        #endregion

        #region helpers

        static async Task<SampleSet> GetSampleSetAsync()
        {
            SampleSet result = Creator.GetSampleSet(_mainVM.SampleSetParameters);
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

    }
}
