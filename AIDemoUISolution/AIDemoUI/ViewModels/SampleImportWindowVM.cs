using AIDemoUI.Commands.Async;
using AIDemoUI.Views;
using DeepLearningDataProvider;
using Microsoft.Win32;
using NeuralNetBuilder;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public interface ISampleImportWindowVM : IBaseVM
    {
        Dictionary<SetName, ISampleSetParameters> Templates { get; }
        ISampleSetParameters SelectedTemplate { get; set; }
        int TestingSamples { get; set; }
        int TrainingSamples { get; set; }
        string Url_TestingImages { get; set; }
        string Url_TestingLabels { get; set; }
        string Url_TrainingImages { get; set; }
        string Url_TrainingLabels { get; set; }
        bool UseAllAvailableTestingSamples { get; set; }
        bool UseAllAvailableTrainingSamples { get; set; }
        bool IsBusy { get; set; }
        string Message { get; }
        IAsyncCommand OkCommand { get; }
        IAsyncCommand SetSamplesLocationCommand { get; }
    }

    public class SampleImportWindowVM : BaseVM, ISampleImportWindowVM
    {
        #region fields & ctor

        ISampleSetParameters selectedSampleSetParameters;
        private readonly ISampleSetSteward _sampleSetSteward;
        bool isBusy;

        public SampleImportWindowVM(ISessionContext sessionContext, ISimpleMediator mediator, 
            ISampleSetSteward sampleSetSteward)
            : base(sessionContext, mediator)
        {
            _sampleSetSteward = sampleSetSteward;

            DefineCommands();
        }

        #region helpers

        private void DefineCommands()
        {
            SetSamplesLocationCommand = new SimpleAsyncRelayCommand(SetSamplesLocationAsync);
            OkCommand = new SimpleAsyncRelayCommand(OkAsync);
        }

        #endregion

        #endregion

        #region properties (No Commands)

        private ISampleSet SampleSet
        {
            get => _sessionContext.SampleSet;
            set => _sessionContext.SampleSet = value;
        }
        private ITrainer Trainer => _sessionContext.Trainer;

        public Dictionary<SetName, ISampleSetParameters> Templates => _sampleSetSteward.DefaultSampleSetParameters;
        public ISampleSetParameters SelectedTemplate
        {
            get { return selectedSampleSetParameters ?? Templates[SetName.FourPixelCamera]; }
            set
            {
                if (selectedSampleSetParameters?.Name != value?.Name)
                {
                    selectedSampleSetParameters = value;
                    // Notify Url_TrainingLabels, Url_TrainingImages, TrainingSamples etc.
                    OnAllPropertiesChanged();
                }
            }
        }
        public string Url_TrainingLabels
        {
            get { return SelectedTemplate.Paths[SampleType.TrainingLabel]; }
            set
            {
                if (SelectedTemplate.Paths[SampleType.TrainingLabel] != value)
                {
                    SelectedTemplate.Paths[SampleType.TrainingLabel] = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Url_TrainingImages
        {
            get { return SelectedTemplate.Paths[SampleType.TrainingData]; }
            set
            {
                if (SelectedTemplate.Paths[SampleType.TrainingData] != value)
                {
                    SelectedTemplate.Paths[SampleType.TrainingData] = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Url_TestingLabels
        {
            get { return SelectedTemplate.Paths[SampleType.TestingLabel]; }
            set
            {
                if (SelectedTemplate.Paths[SampleType.TestingLabel] != value)
                {
                    SelectedTemplate.Paths[SampleType.TestingLabel] = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Url_TestingImages
        {
            get { return SelectedTemplate.Paths[SampleType.TestingData]; }
            set
            {
                if (SelectedTemplate.Paths[SampleType.TestingData] != value)
                {
                    SelectedTemplate.Paths[SampleType.TestingData] = value;
                    OnPropertyChanged();
                }
            }
        }
        public int TrainingSamples
        {
            get { return SelectedTemplate.TrainingSamples; }
            set
            {
                if (SelectedTemplate.TrainingSamples != value)
                {
                    SelectedTemplate.TrainingSamples = value;
                    UseAllAvailableTrainingSamples = false;
                    OnPropertyChanged();
                }
            }
        }
        public int TestingSamples
        {
            get { return SelectedTemplate.TestingSamples; }
            set
            {
                if (SelectedTemplate.TestingSamples != value)
                {
                    SelectedTemplate.TestingSamples = value;
                    UseAllAvailableTestingSamples = false;
                    OnPropertyChanged();
                }
            }
        }
        public bool UseAllAvailableTrainingSamples
        {
            get { return SelectedTemplate.UseAllAvailableTrainingSamples; }
            set
            {
                if (SelectedTemplate.UseAllAvailableTrainingSamples != value)
                {
                    if (value == true)
                    {
                        TrainingSamples = SelectedTemplate.DefaultTrainingSamples;
                        OnPropertyChanged(nameof(TrainingSamples));
                    }
                    SelectedTemplate.UseAllAvailableTrainingSamples = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool UseAllAvailableTestingSamples
        {
            get { return SelectedTemplate.UseAllAvailableTestingSamples; }
            set
            {
                if (SelectedTemplate.UseAllAvailableTestingSamples != value)
                {
                    if (value == true)
                    {
                        TestingSamples = SelectedTemplate.DefaultTestingSamples;
                        OnPropertyChanged(nameof(TestingSamples));
                    }
                    SelectedTemplate.UseAllAvailableTestingSamples = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Message => _sampleSetSteward.Message;

        #endregion

        #region Commands

        public IAsyncCommand SetSamplesLocationCommand { get; private set; }
        public IAsyncCommand OkCommand { get; private set; }

        #region Executes and CanExecutes

        private async Task SetSamplesLocationAsync(object parameter)
        {
            string url = parameter as string;

            if (!string.IsNullOrEmpty(url))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    await Task.Run(() =>
                    {
                        switch (url)
                        {
                            case nameof(Url_TrainingLabels):
                                Url_TrainingLabels = openFileDialog.FileName;
                                break;
                            case nameof(Url_TrainingImages):
                                Url_TrainingImages = openFileDialog.FileName;
                                break;
                            case nameof(Url_TestingLabels):
                                Url_TestingLabels = openFileDialog.FileName;
                                break;
                            case nameof(Url_TestingImages):
                                Url_TestingImages = openFileDialog.FileName;
                                break;
                            default:
                                break;
                        }
                    });
                }
            }
        }
        private async Task OkAsync(object parameter)
        {
            // Json Serialize:
            var jsonString = JsonConvert.SerializeObject(SelectedTemplate, Formatting.Indented);
            var path = @"C:\Users\Jan_PC\Documents\_NeuralNetApp\Saves\ConsoleApi_SampleSetParameters.txt";
            await File.WriteAllTextAsync(path, jsonString);



            IsBusy = true;
            SampleSet = await _sampleSetSteward.CreateSampleSetAsync(SelectedTemplate);

            // Json Serialize:
            jsonString = JsonConvert.SerializeObject(SampleSet, Formatting.Indented);
            path = @"C:\Users\Jan_PC\Documents\_NeuralNetApp\Saves\ConsoleApi_SampleSet.txt";
            await File.WriteAllTextAsync(path, jsonString);

            (parameter as SampleImportWindow)?.Hide();  // via DelegateFactory?

            if (Trainer.TrainerStatus != TrainerStatus.Undefined)
            {
                Trainer.SampleSet = SampleSet;
            }
            IsBusy = false;
            _mediator.NotifyColleagues(MediatorToken.StartStopVM_OnSampleSetInitialized.ToString(), null);
        }

        #endregion

        #endregion
    }
}
