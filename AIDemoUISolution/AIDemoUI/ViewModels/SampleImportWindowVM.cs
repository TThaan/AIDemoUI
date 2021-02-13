using AIDemoUI.Commands;
using AIDemoUI.Views;
using DeepLearningDataProvider;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public class SampleImportWindowVM : BaseSubVM
    {
        #region ctor & fields

        IAsyncCommand okCommandAsync, setSamplesLocationCommandAsync;
        SampleSetParameters selectedSampleSetParameters;
        private readonly ISessionContext _sessionContext;

        public SampleImportWindowVM(ISessionContext sessionContext)
        {
            SetDefaultValues();
            _sessionContext = sessionContext;
        }

        #region helpers

        void SetDefaultValues()
        {
            SelectedTemplate = Templates.Values.First();
        }

        #endregion

        #endregion

        #region public
        
        public Dictionary<SetName, SampleSetParameters> Templates => Creator.Templates;
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

        #endregion

        #region RelayCommand

        public IAsyncCommand SetSamplesLocationCommandAsync
        {
            get
            {
                if (setSamplesLocationCommandAsync == null)
                {
                    setSamplesLocationCommandAsync = new AsyncRelayCommand(SetSamplesLocationCommand_Execute, SetSamplesLocationCommand_CanExecute);
                }
                return setSamplesLocationCommandAsync;
            }
        }
        async Task SetSamplesLocationCommand_Execute(object parameter)
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
        bool SetSamplesLocationCommand_CanExecute(object parameter)
        {
            return true;
        }
        public IAsyncCommand OkCommandAsync
        {
            get
            {
                if (okCommandAsync == null)
                {
                    okCommandAsync = new AsyncRelayCommand(OkCommandAsync_Execute, OkCommandAsync_CanExecute);
                }
                return okCommandAsync;
            }
        }
        async Task OkCommandAsync_Execute(object parameter)
        {
            (parameter as SampleImportWindow)?.Hide();

            // Use a local variable to store the sample set in.
            // So the TrainCommandAsync_CanExecute (enabling the 'Train' button) won't return 'true' too early.
            SampleSet sampleSet = Creator.GetSampleSet(SelectedTemplate);
            // sampleSet.StatusChanged += _sessionContext.StatusVM.SampleSet_StatusChanged;  // DIC
            await sampleSet.SetSamples();
            _sessionContext.StartStopVM.SampleSet = sampleSet;
        }
        bool OkCommandAsync_CanExecute(object parameter)
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
