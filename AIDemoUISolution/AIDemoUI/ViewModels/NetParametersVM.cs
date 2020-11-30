using AIDemoUI.Commands;
using FourPixCam;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AIDemoUI.ViewModels
{
    public delegate Task OkBtnEventHandler(NetParameters netParameters, bool isTurnBased, 
        string Url_TrainingLabels, string Url_TrainingImages, string Url_TestingLabels, string Url_TestingImages);

    public class NetParametersVM : BaseVM
    {
        #region ctor & fields

        NetParameters _netParameters;
        IRelayCommand addCommand, deleteCommand, moveLeftCommand, moveRightCommand, unfocusCommand;
        IAsyncCommand okCommandAsync, loadNetCommandAsync, saveNetCommandAsync, setSamplesLocationCommandAsync;
        IEnumerable<ActivationType> activationTypes;
        IEnumerable<CostType> costTypes;
        IEnumerable<WeightInitType> weightInitTypes;
        ObservableCollection<LayerVM> layerVMs;
        ObservableCollection<string> sampleSets;
        string selectedSampleSet, url_TrainingLabels, url_TrainingImages, url_TestingImages, url_TestingLabels;

        public NetParametersVM()
        {
            _netParameters = new NetParameters();
            SetDefaultValues();
            // SampleSetChanged += OnSampleSetChanged;
            LayerVMs.CollectionChanged += OnLayerVMsChanged;
        }

        #region helpers

        void SetDefaultValues()
        {
            IsWithBias = false;
            WeightMin = -1;
            WeightMax = 1;
            BiasMin = -1;
            BiasMax = 1;
            CostType = CostType.SquaredMeanError;
            WeightInitType = WeightInitType.Xavier;
            LayerVMs = new ObservableCollection<LayerVM>
            {
                new LayerVM(new Layer(){ Id = 0}),
                new LayerVM(new Layer(){ Id = 1}),
                new LayerVM(new Layer(){ Id = 2})
            };

            SampleSets = new ObservableCollection<string>(
                new[]
                {
                    "No Sample Selected", "Four Pixel Camera", "MNIST"
                });
            SelectedSampleSet = sampleSets.First();
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
        public ObservableCollection<LayerVM> LayerVMs
        {
            get { return layerVMs; }
            set
            {
                if (layerVMs != value)
                {
                    layerVMs = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsWithBias
        {
            get { return _netParameters.IsWithBias; }
            set
            {
                if (_netParameters.IsWithBias != value)
                {
                    _netParameters.IsWithBias = value;
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMin
        {
            get { return _netParameters.WeightMin; }
            set
            {
                if (_netParameters.WeightMin != value)
                {
                    _netParameters.WeightMin = value;
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMax
        {
            get { return _netParameters.WeightMax; }
            set
            {
                if (_netParameters.WeightMax != value)
                {
                    _netParameters.WeightMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMin
        {
            get { return _netParameters.BiasMin; }
            set
            {
                if (_netParameters.BiasMin != value)
                {
                    _netParameters.BiasMin = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMax
        {
            get { return _netParameters.BiasMax; }
            set
            {
                if (_netParameters.BiasMax != value)
                {
                    _netParameters.BiasMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public CostType CostType
        {
            get { return _netParameters.CostType; }
            set
            {
                if (_netParameters.CostType != value)
                {
                    _netParameters.CostType = value;
                    OnPropertyChanged();
                }
            }
        }
        public WeightInitType WeightInitType
        {
            get { return _netParameters.WeightInitType; }
            set
            {
                if (_netParameters.WeightInitType != value)
                {
                    _netParameters.WeightInitType = value;
                    OnPropertyChanged();
                }
            }
        }

        public IEnumerable<ActivationType> ActivationTypes => activationTypes ??
            (activationTypes = Enum.GetValues(typeof(ActivationType)).ToList<ActivationType>().Skip(1));
        public IEnumerable<CostType> CostTypes => costTypes ??
            (costTypes = Enum.GetValues(typeof(CostType)).ToList<CostType>().Skip(1));
        public IEnumerable<WeightInitType> WeightInitTypes => weightInitTypes ??
            (weightInitTypes = Enum.GetValues(typeof(WeightInitType)).ToList<WeightInitType>().Skip(1));

        #endregion

        #region RelayCommand

        #region unspecified Commands

        public IRelayCommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(AddCommand_Execute, AddCommand_CanExecute);
                }
                return addCommand;
            }
        }
        void AddCommand_Execute(object parameter)
        {
            ContentPresenter cp = parameter as ContentPresenter;
            LayerVM layerVM = cp.Content as LayerVM;

            LayerVM newLayerVM = new LayerVM(new Layer() { ActivationType = ActivationType.ReLU });
            int newIndex = (LayerVMs.IndexOf(layerVM));
            LayerVMs.Insert(newIndex + 1, newLayerVM);
        }
        bool AddCommand_CanExecute(object parameter)
        {
            return true;
        }
        public IRelayCommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(DeleteCommand_Execute, DeleteCommand_CanExecute);
                }
                return deleteCommand;
            }
        }
        void DeleteCommand_Execute(object parameter)
        {
            ContentPresenter cp = parameter as ContentPresenter;
            LayerVM layerVM = cp.Content as LayerVM;
            LayerVMs.Remove(layerVM);
        }
        bool DeleteCommand_CanExecute(object parameter)
        {
            return true;
        }
        public IRelayCommand MoveLeftCommand
        {
            get
            {
                if (moveLeftCommand == null)
                {
                    moveLeftCommand = new RelayCommand(MoveLeftCommand_Execute, MoveLeftCommand_CanExecute);
                }
                return moveLeftCommand;
            }
        }
        void MoveLeftCommand_Execute(object parameter)
        {
            ContentPresenter cp = parameter as ContentPresenter;
            LayerVM layerVM = cp.Content as LayerVM;
            int currentIndex = LayerVMs.IndexOf(layerVM);
            LayerVMs.Move(currentIndex, currentIndex > 0 ? currentIndex - 1 : 0);
        }
        bool MoveLeftCommand_CanExecute(object parameter)
        {
            return true;
        }
        public IRelayCommand MoveRightCommand
        {
            get
            {
                if (moveRightCommand == null)
                {
                    moveRightCommand = new RelayCommand(MoveRightCommand_Execute, MoveRightCommand_CanExecute);
                }
                return moveRightCommand;
            }
        }
        void MoveRightCommand_Execute(object parameter)
        {
            ContentPresenter cp = parameter as ContentPresenter;
            LayerVM layerVM = cp.Content as LayerVM;
            int currentIndex = LayerVMs.IndexOf(layerVM);
            LayerVMs.Move(currentIndex, currentIndex < LayerVMs.Count - 1 ? currentIndex + 1 : 0);
        }
        bool MoveRightCommand_CanExecute(object parameter)
        {
            return true;
        }
        public IRelayCommand UnfocusCommand
        {
            get
            {
                if (unfocusCommand == null)
                {
                    unfocusCommand = new RelayCommand(UnfocusCommand_Execute, UnfocusCommand_CanExecute);
                }
                return unfocusCommand;
            }
        }
        void UnfocusCommand_Execute(object parameter)
        {
            var netParametersView = parameter as UserControl;
            netParametersView.FocusVisualStyle = null;
            netParametersView.Focusable = true;
            netParametersView.Focus();
        }
        bool UnfocusCommand_CanExecute(object parameter)
        {
            return true;
        }
        public IAsyncCommand OkCommandAsync
        {
            get
            {
                if (okCommandAsync == null)
                {
                    okCommandAsync = new AsyncRelayCommand(OkCommand_Execute, OkCommand_CanExecute);
                }
                return okCommandAsync;
            }
        }
        async Task OkCommand_Execute(object parameter)
        {
            var okBtn = parameter as Button;
            // var netParametersView = parameter as UserControl;
            if (okBtn != null)
            {
                string isTurnBased = okBtn.Content as string;
                if ((isTurnBased?.ToLower()).Contains("step"))
                {
                    await OnOkBtnPressedAsync(true);
                }
                else
                {
                    await OnOkBtnPressedAsync(false);
                }
            }
        }
        bool OkCommand_CanExecute(object parameter)
        {
            if (string.IsNullOrEmpty(Url_TrainingLabels) ||
                string.IsNullOrEmpty(Url_TrainingImages) ||
                string.IsNullOrEmpty(Url_TestingLabels) ||
                string.IsNullOrEmpty(Url_TestingImages))
            {
                return false;
            }
            return true;
        }

        #endregion

        #region I/O

        #region Change SampleSet

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
        //public IAsyncCommand LoadTestingDataFromUrlCommandAsync
        //{
        //    get
        //    {
        //        if (loadTestingDataFromUrlCommandAsync == null)
        //        {
        //            loadTestingDataFromUrlCommandAsync = new AsyncRelayCommand(LoadTestingDataFromUrlCommand_Execute, LoadTestingDataFromUrlCommand_CanExecute);
        //        }
        //        return loadTestingDataFromUrlCommandAsync;
        //    }
        //}
        //async Task LoadTestingDataFromUrlCommand_Execute(object parameter)
        //{
        //    // Either download from url
        //    using (var client = new WebClient())
        //    {
        //        client.DownloadFile("http://yann.lecun.com/exdb/mnist/t10k-images.idx3-ubyte.gz", "abc.gz");
        //    }
        //    // or use stream directly...

        //    throw new NotImplementedException();
        //}
        //bool LoadTestingDataFromUrlCommand_CanExecute(object parameter)
        //{
        //    return true;
        //}

        #endregion

        #region Load/Save Net

        public IAsyncCommand LoadNetCommandAsync
        {
            get
            {
                if (loadNetCommandAsync == null)
                {
                    loadNetCommandAsync = new AsyncRelayCommand(LoadNetCommand_Execute, LoadNetCommand_CanExecute);
                }
                return loadNetCommandAsync;
            }
        }
        async Task LoadNetCommand_Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    Stream stream = openFileDialog.OpenFile();
                    BinaryFormatter b = new BinaryFormatter();
                    NetParameters np = (NetParameters)b.Deserialize(stream);
                    SetLoadedValues(np);
                });
            }
        }
        bool LoadNetCommand_CanExecute(object parameter)
        {
            return true;
        }
        public IAsyncCommand SaveNetCommandAsync
        {
            get
            {
                if (saveNetCommandAsync == null)
                {
                    saveNetCommandAsync = new AsyncRelayCommand(SaveNetCommand_Execute, SaveNetCommand_CanExecute);
                }
                return saveNetCommandAsync;
            }
        }
        async Task SaveNetCommand_Execute(object parameter)
        {
            _netParameters.Layers = LayerVMs
                .Select(x => x.Layer)
                .ToArray();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save this Template";
            saveFileDialog.DefaultExt = ".txt";
            // saveFileDialog.Filter = "Text| *.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    await Task.Run(() =>
                    {
                        Stream stream = saveFileDialog.OpenFile();
                        BinaryFormatter b = new BinaryFormatter();
                        b.Serialize(stream, _netParameters);
                        stream.Close();
                    });
                }
            }
        }
        bool SaveNetCommand_CanExecute(object parameter)
        {
            return true;
        }

        #region helpers

        // in NP class:
        void SerializeNetParameters()
        {
            _netParameters.Layers = LayerVMs
                .Select(x => x.Layer)
                .ToArray();
            Stream stream = File.Open("temp.dat", FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(stream, _netParameters);
            stream.Close();
        }
        void DeSerializeNetParameters()
        {
            Stream stream = File.Open("temp.dat", FileMode.Open);
            BinaryFormatter b = new BinaryFormatter();
            _netParameters = (NetParameters)b.Deserialize(stream);
            stream.Close();
        }
        void SetLoadedValues(NetParameters np)
        {
            IsWithBias = np.IsWithBias;
            WeightMin = np.WeightMin;
            WeightMax = np.WeightMax;
            BiasMin = np.BiasMin;
            BiasMax = np.BiasMax;
            CostType = np.CostType;
            WeightInitType = np.WeightInitType;
            LayerVMs = new ObservableCollection<LayerVM>(
                np.Layers.Select(x => new LayerVM(x)));
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region OnLayersChanged

        void OnLayerVMsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // Check if input layer has changed.
                    // If 'yes': Notify 'InputValues'.
                    break;
                case NotifyCollectionChangedAction.Remove:
                    // Check if input layer has changed.
                    // If 'yes': Notify 'InputValues'.
                    break;
                case NotifyCollectionChangedAction.Replace:
                    // Check if input layer has changed.
                    // If 'yes': Notify 'InputValues'.
                    break;
                case NotifyCollectionChangedAction.Move:
                    // Check if input layer has changed.
                    // If 'yes': Notify 'InputValues'.
                    break;
                case NotifyCollectionChangedAction.Reset:
                    // Check if input layer has changed.
                    // If 'yes': Notify 'InputValues'.
                    break;
                default:
                    break;
            }
            UpdateIndeces();
        }

        #region helpers

        void UpdateIndeces()
        {
            for (int i = 0; i < LayerVMs.Count; i++)
            {
                LayerVMs.ElementAt(i).Id = i;
                for (int k = 0; k < LayerVMs.ElementAt(i).Inputs.Count; k++)
                {
                    LayerVMs.ElementAt(i).Inputs[k] = 0f;
                }
            }
        }

        #endregion

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

        #region OkBtnPressed

        public event OkBtnEventHandler OkBtnPressed;
        async Task OnOkBtnPressedAsync(bool isTurnBased)
        {
            _netParameters.Layers = LayerVMs
                .Select(x => x.Layer)
                .ToArray();
            await OkBtnPressed?.Invoke(_netParameters, isTurnBased, Url_TrainingLabels, Url_TrainingImages, Url_TestingLabels, Url_TestingImages);
        }

        #endregion
    }
}
