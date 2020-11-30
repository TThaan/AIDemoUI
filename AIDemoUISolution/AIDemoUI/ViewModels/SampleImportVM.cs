using AIDemoUI.Commands;
using AIDemoUI.Views;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public class SampleImportVM : BaseVM
    {
        #region ctor & fields

        IRelayCommand okCommand;
        IAsyncCommand setSamplesLocationCommandAsync;
        ObservableCollection<string> sampleSets;
        string selectedSampleSet, url_TrainingLabels, url_TrainingImages, url_TestingImages, url_TestingLabels;
        int trainingSamples, testingSamples;
        bool useAllAvailableTrainingSamples, useAllAvailableTestingSamples;

        public SampleImportVM()
        {
            SetDefaultValues();
        }

        #region helpers

        void SetDefaultValues()
        {
            SampleSets = new ObservableCollection<string>(
                new[]
                {
                    "No Sample Selected", "Four Pixel Camera", "MNIST"
                });
            SelectedSampleSet = sampleSets.First();
            UseAllAvailableTrainingSamples = true;
            UseAllAvailableTestingSamples = true;
        }

        #endregion

        #endregion

        #region public
        
        public ObservableCollection<string> SampleSets
        {
            get { return sampleSets; }
            set
            {
                if (sampleSets != value)
                {
                    sampleSets = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SelectedSampleSet
        {
            get { return selectedSampleSet; }
            set
            {
                if (selectedSampleSet != value)
                {
                    selectedSampleSet = value;
                    OnSampleSetChanged();
                    OnPropertyChanged();
                }
            }
        }
        public string Url_TrainingLabels
        {
            get { return url_TrainingLabels; }
            set
            {
                if (url_TrainingLabels != value)
                {
                    url_TrainingLabels = value;
                    // OnSampleSetChanged();
                    OnPropertyChanged();
                }
            }
        }
        public string Url_TrainingImages
        {
            get { return url_TrainingImages; }
            set
            {
                if (url_TrainingImages != value)
                {
                    url_TrainingImages = value;
                    // OnSampleSetChanged();
                    OnPropertyChanged();
                }
            }
        }
        public string Url_TestingLabels
        {
            get { return url_TestingLabels; }
            set
            {
                if (url_TestingLabels != value)
                {
                    url_TestingLabels = value;
                    // OnSampleSetChanged();
                    OnPropertyChanged();
                }
            }
        }
        public string Url_TestingImages
        {
            get { return url_TestingImages; }
            set
            {
                if (url_TestingImages != value)
                {
                    url_TestingImages = value;
                    // OnSampleSetChanged();
                    OnPropertyChanged();
                }
            }
        }
        public int TrainingSamples
        {
            get { return trainingSamples; }
            set
            {
                if (trainingSamples != value)
                {
                    trainingSamples = value;
                    // OnSampleSetChanged();
                    OnPropertyChanged();
                }
            }
        }
        public int TestingSamples
        {
            get { return testingSamples; }
            set
            {
                if (testingSamples != value)
                {
                    testingSamples = value;
                    // OnSampleSetChanged();
                    OnPropertyChanged();
                }
            }
        }
        public bool UseAllAvailableTrainingSamples
        {
            get { return useAllAvailableTrainingSamples; }
            set
            {
                if (useAllAvailableTrainingSamples != value)
                {
                    useAllAvailableTrainingSamples = value;
                    // OnSampleSetChanged();
                    OnPropertyChanged();
                }
            }
        }
        public bool UseAllAvailableTestingSamples
        {
            get { return useAllAvailableTestingSamples; }
            set
            {
                if (useAllAvailableTestingSamples != value)
                {
                    useAllAvailableTestingSamples = value;
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
        public IRelayCommand OkCommand
        {
            get
            {
                if (okCommand == null)
                {
                    okCommand = new RelayCommand(OkCommand_Execute, OkCommand_CanExecute);
                }
                return okCommand;
            }
        }
        void OkCommand_Execute(object parameter)
        {
            (parameter as SampleImportWindow)?.Hide();
        }
        bool OkCommand_CanExecute(object parameter)
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
            switch (propertyName)
            {
                case nameof(SelectedSampleSet):
                    SelectSampleSet();
                    break;
                default:
                    break;
            }

            OnPropertyChanged(propertyName);
        }

        #region helpers

        void SelectSampleSet()
        {
            switch (SelectedSampleSet)
            {
                case "No Sample Selected":
                    Url_TrainingLabels = default;
                    Url_TrainingImages = default;
                    Url_TestingLabels = default;
                    Url_TestingImages = default;
                    break;
                case "Four Pixel Camera":
                    Url_TrainingLabels = new NNet_InputProvider.FourPixCam.DataFactory().Url_TrainLabels;
                    Url_TrainingImages = new NNet_InputProvider.FourPixCam.DataFactory().Url_TrainImages;
                    Url_TestingLabels = new NNet_InputProvider.FourPixCam.DataFactory().Url_TestLabels;
                    Url_TestingImages = new NNet_InputProvider.FourPixCam.DataFactory().Url_TestImages;
                    break;
                case "MNIST":
                    Url_TrainingLabels = new NNet_InputProvider.MNIST.DataFactory().Url_TrainLabels;
                    Url_TrainingImages = new NNet_InputProvider.MNIST.DataFactory().Url_TrainImages;
                    Url_TestingLabels = new NNet_InputProvider.MNIST.DataFactory().Url_TestLabels;
                    Url_TestingImages = new NNet_InputProvider.MNIST.DataFactory().Url_TestImages;
                    break;
                default:
                    Url_TrainingLabels = default;
                    Url_TrainingImages = default;
                    Url_TestingLabels = default;
                    Url_TestingImages = default;
                    break;
            }
        }

        #endregion

        #endregion
    }
}
