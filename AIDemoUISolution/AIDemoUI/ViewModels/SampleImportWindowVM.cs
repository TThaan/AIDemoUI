using AIDemoUI.Commands;
using AIDemoUI.Views;
using DeepLearningDataProvider;
using Microsoft.Win32;
using NeuralNetBuilder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public interface ISampleImportWindowVM : IBaseVM
    {
        Dictionary<SetName, SampleSetParameters> Templates { get; }
        ObservableCollection<SetName> TemplateNames { get; }
        SampleSetParameters SelectedTemplate { get; set; }
        int TestingSamples { get; set; }
        int TrainingSamples { get; set; }
        string Url_TestingImages { get; set; }
        string Url_TestingLabels { get; set; }
        string Url_TrainingImages { get; set; }
        string Url_TrainingLabels { get; set; }
        bool UseAllAvailableTestingSamples { get; set; }
        bool UseAllAvailableTrainingSamples { get; set; }
        IAsyncCommand OkCommand { get; set; }
        IAsyncCommand SetSamplesLocationCommand { get; set; }
        Task OkAsync(object parameter);
        bool OkAsync_CanExecute(object parameter);
        Task SetSamplesLocationAsync(object parameter);
        bool SetSamplesLocationAsync_CanExecute(object parameter);
    }

    public class SampleImportWindowVM : BaseSubVM, ISampleImportWindowVM
    {
        #region fields & ctor

        SampleSetParameters selectedSampleSetParameters;
        private readonly ISessionContext _sessionContext;
        private readonly ISamplesSteward _samplesSteward;

        public SampleImportWindowVM(ISessionContext sessionContext, ISimpleMediator mediator, ISamplesSteward samplesSteward)
            : base(mediator)
        {
            _sessionContext = sessionContext;
            _samplesSteward = samplesSteward;

            _mediator.Register("Token: MainWindowVM", SampleImportWindowVMCallback);
        }
        private void SampleImportWindowVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region properties

        private SampleSet SampleSet
        {
            get => _sessionContext.SampleSet;
            set => _sessionContext.SampleSet = value;
        }
        private bool IsSampleSetInitialized
        {
            get => _sessionContext.IsSampleSetInitialized;
            set => _sessionContext.IsSampleSetInitialized = value;
        }
        private ITrainer Trainer => _sessionContext.Trainer;
        private bool IsTrainerInitialized => _sessionContext.IsTrainerInitialized;

        public Dictionary<SetName, SampleSetParameters> Templates => _samplesSteward.Templates;
        public ObservableCollection<SetName> TemplateNames => Templates.Keys.ToObservableCollection();
        public SampleSetParameters SelectedTemplate
        {
            get { return selectedSampleSetParameters; }
            set
            {
                if (selectedSampleSetParameters?.Name != value?.Name)
                {
                    selectedSampleSetParameters = value;
                    OnSampleSetChanged();
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
                    // OnSampleSetChanged();
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
                    // OnSampleSetChanged();
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
                    SelectedTemplate.UseAllAvailableTrainingSamples = value;
                    // OnSampleSetChanged();
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
                    SelectedTemplate.UseAllAvailableTestingSamples = value;
                    // OnSampleSetChanged();
                    OnPropertyChanged();
                }
            }
        }

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
            IsSampleSetInitialized = false;
            (parameter as SampleImportWindow)?.Hide();
            SampleSet = await _samplesSteward.CreateSampleSetAsync(SelectedTemplate);   // Use mediator here? Like: _mediator.GetSampleSet_StatusChanged()..
            IsSampleSetInitialized = true;

            if (IsTrainerInitialized)
            {
                Trainer.SampleSet = SampleSet;
            }
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

        #endregion

        #region SampleSetChanged

        void OnSampleSetChanged([CallerMemberName] string propertyName = null)
        {
            // redundant..
            OnPropertyChanged("Url_TrainingLabels");
            OnPropertyChanged("Url_TrainingImages");
            OnPropertyChanged("Url_TestingLabels");
            OnPropertyChanged("Url_TestingImages");
        }

        #endregion
    }
}
