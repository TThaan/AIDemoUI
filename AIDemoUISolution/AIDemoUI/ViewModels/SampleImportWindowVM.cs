using AIDemoUI.Commands;
using AIDemoUI.Views;
using DeepLearningDataProvider;
using Microsoft.Win32;
using NeuralNetBuilder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public interface ISampleImportWindowVM : IBaseVM
    {
        Dictionary<SetName, ISampleSetParameters> Templates { get; }
        ObservableCollection<SetName> TemplateNames { get; }
        ISampleSetParameters SelectedTemplate { get; set; }
        int TestingSamples { get; set; }
        int TrainingSamples { get; set; }
        string Url_TestingImages { get; set; }
        string Url_TestingLabels { get; set; }
        string Url_TrainingImages { get; set; }
        string Url_TrainingLabels { get; set; }
        bool UseAllAvailableTestingSamples { get; set; }
        bool UseAllAvailableTrainingSamples { get; set; }
        bool IsBusy { get; }
        string Message { get; }
        IAsyncCommand OkCommand { get; set; }
        IAsyncCommand SetSamplesLocationCommand { get; set; }
        Task OkAsync(object parameter);
        bool OkAsync_CanExecute(object parameter);
        Task SetSamplesLocationAsync(object parameter);
        bool SetSamplesLocationAsync_CanExecute(object parameter);
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
            _mediator.Register("Token: MainWindowVM", SampleImportWindowVMCallback);
        }
        private void SampleImportWindowVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region properties (No Commands)

        private ISampleSet SampleSet
        {
            get => _sessionContext.SampleSet;
            set => _sessionContext.SampleSet = value;
        }
        private ITrainer Trainer => _sessionContext.Trainer;

        public Dictionary<SetName, ISampleSetParameters> Templates => _sampleSetSteward.DefaultSampleSetParameters;
        public ObservableCollection<SetName> TemplateNames => Templates.Keys.ToObservableCollection();
        public ISampleSetParameters SelectedTemplate
        {
            get { return selectedSampleSetParameters ?? Templates[SetName.FourPixelCamera]; }
            set
            {
                if (selectedSampleSetParameters?.Name != value?.Name)
                {
                    selectedSampleSetParameters = value;
                    OnPropertyChanged();
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

        public IAsyncCommand SetSamplesLocationCommand { get; set; }
        public IAsyncCommand OkCommand { get; set; }

        #region Executes and CanExecutes

        public async Task SetSamplesLocationAsync(object parameter)
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
        public bool SetSamplesLocationAsync_CanExecute(object parameter)
        {
            return true;
        }
        public async Task OkAsync(object parameter)
        {
            IsBusy = true;
            SampleSet = await _sampleSetSteward.CreateSampleSetAsync(SelectedTemplate);   // Use mediator here? Like: _mediator.GetSampleSet_StatusChanged()..
            (parameter as SampleImportWindow)?.Hide();

            if (Trainer.TrainerStatus != TrainerStatus.Undefined)
            {
                Trainer.SampleSet = SampleSet;
            }
            IsBusy = false;
            // Set or Notify StartStopVM...ButtonText
        }
        public bool OkAsync_CanExecute(object parameter)
        {
            //if (string.IsNullOrEmpty(Url_TrainingLabels) ||
            //    string.IsNullOrEmpty(Url_TrainingImages) ||
            //    string.IsNullOrEmpty(Url_TestingLabels) ||
            //    string.IsNullOrEmpty(Url_TestingImages))
            //{
            //    return false;
            //}
            return true;
        }

        #endregion

        #endregion
    }
}
