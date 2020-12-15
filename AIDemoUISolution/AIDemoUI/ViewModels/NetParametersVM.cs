using AIDemoUI.Commands;
using AIDemoUI.Views;
using Microsoft.Win32;
using NeuralNetBuilder.FactoriesAndParameters;
using NNet_InputProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public delegate Task OkBtnEventHandler(INetParameters netParameters, bool isTurnBased, SampleSetParameters sampleSetParameters);

    public class NetParametersVM : BaseVM
    {
        #region ctor & fields

        INetParameters _netParameters;
        ITrainerParameters _trainerParameters;
        IRelayCommand importSamplesCommand;
        IAsyncCommand loadNetCommandAsync, saveNetCommandAsync;
        IEnumerable<ActivationType> activationTypes;
        IEnumerable<CostType> costTypes;
        IEnumerable<WeightInitType> weightInitTypes;
        ObservableCollection<LayerParametersVM> layerParameterVMs;
        bool isWithBias_Global, areParametersGlobal;
        float weightMin_Global, weightMax_Global, biasMin_Global, biasMax_Global;

        public NetParametersVM(INetParameters netParameters, ITrainerParameters trainerParameters)
        {
            _netParameters = netParameters ?? throw new NullReferenceException(
                    $"{typeof(INetParameters).Name} {nameof(netParameters)} ({typeof(NetParametersVM).Name}.ctor)");
            _trainerParameters = trainerParameters ?? throw new NullReferenceException(
                    $"{typeof(ITrainerParameters).Name} {nameof(trainerParameters)} ({typeof(NetParametersVM).Name}.ctor)");
            SampleImportWindow = new SampleImportWindow();

            SetDefaultValues();
            LayerParameterVMs.CollectionChanged += OnLayerParameterVMsChanged;
        }

        #region helpers

        void SetDefaultValues()
        {
            AreParametersGlobal = true;
            IsWithBias_Global = false;
            WeightMin_Global = -1;
            WeightMax_Global = 1;
            BiasMin_Global = -1;
            BiasMax_Global = 1;
            CostType = CostType.SquaredMeanError;
            WeightInitType = WeightInitType.Xavier;
            LayerParameterVMs = new ObservableCollection<LayerParametersVM>
            {
                new LayerParametersVM(0),
                new LayerParametersVM(1),
                new LayerParametersVM(2),
                new LayerParametersVM(3),
                new LayerParametersVM(4)
            };
            foreach (var layerParameterVM in LayerParameterVMs)
            {
                layerParameterVM.PropertyChanged += LayerParametersVM_PropertyChanged;
            }
            LearningRate = .1f;
            LearningRateChange = .9f;
            EpochCount = 10;
        }

        private void LayerParametersVM_PropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            string suffix = "_Execute";

            switch (eventArgs.PropertyName.Replace(suffix, ""))
            {
                case nameof(LayerParametersVM.AddCommand):
                    AddLayerParametersVM(sender as LayerParametersVM);
                    break;
                case nameof(LayerParametersVM.DeleteCommand):
                    DeleteLayerParametersVM(sender as LayerParametersVM);
                    break;
                case nameof(LayerParametersVM.MoveLeftCommand):
                    MoveLayerParametersVMLeft(sender as LayerParametersVM);
                    break;
                case nameof(LayerParametersVM.MoveRightCommand):
                    MoveLayerParametersVMRight(sender as LayerParametersVM);
                    break;
                default:
                    break;
            }
        }

        #region helpers

        void AddLayerParametersVM(LayerParametersVM layerParametersVM)
        {
            int newIndex = LayerParameterVMs.IndexOf(layerParametersVM) + 1;
            LayerParametersVM newLayerParametersVM = new LayerParametersVM(newIndex);
            newLayerParametersVM.PropertyChanged += LayerParametersVM_PropertyChanged;
            LayerParameterVMs.Insert(newIndex, newLayerParametersVM);
        }
        void DeleteLayerParametersVM(LayerParametersVM layerParametersVM)
        {
            LayerParameterVMs.Remove(layerParametersVM);
        }
        void MoveLayerParametersVMLeft(LayerParametersVM layerParametersVM)
        {
            int currentIndex = LayerParameterVMs.IndexOf(layerParametersVM);
            LayerParameterVMs.Move(currentIndex, currentIndex > 0 ? currentIndex - 1 : 0);
        }
        void MoveLayerParametersVMRight(LayerParametersVM layerParametersVM)
        {
            int currentIndex = LayerParameterVMs.IndexOf(layerParametersVM);
            LayerParameterVMs.Move(currentIndex, currentIndex < LayerParameterVMs.Count - 1 ? currentIndex + 1 : 0);
        }

        #endregion

        #endregion

        #endregion

        #region public

        public SampleImportWindow SampleImportWindow { get; set; }
        public ObservableCollection<LayerParametersVM> LayerParameterVMs
        {
            get { return layerParameterVMs; }
            set
            {
                if (layerParameterVMs != value)
                {
                    layerParameterVMs = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsWithBias_Global
        {
            get { return isWithBias_Global; }
            set
            {
                if (isWithBias_Global != value)
                {
                    isWithBias_Global = value;
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMin_Global
        {
            get { return weightMin_Global; }
            set
            {
                if (weightMin_Global != value)
                {
                    weightMin_Global = value;
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMax_Global
        {
            get { return weightMax_Global; }
            set
            {
                if (weightMax_Global != value)
                {
                    weightMax_Global = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMin_Global
        {
            get { return biasMin_Global; }
            set
            {
                if (biasMin_Global != value)
                {
                    biasMin_Global = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMax_Global
        {
            get { return biasMax_Global; }
            set
            {
                if (biasMax_Global != value)
                {
                    biasMax_Global = value;
                    OnPropertyChanged();
                }
            }
        }
        public CostType CostType
        {
            get { return _trainerParameters.CostType; }
            set
            {
                if (_trainerParameters.CostType != value)
                {
                    _trainerParameters.CostType = value;
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
            (activationTypes = Enum.GetValues(typeof(ActivationType)).ToList<ActivationType>());
        public IEnumerable<CostType> CostTypes => costTypes ??
            (costTypes = Enum.GetValues(typeof(CostType)).ToList<CostType>());
        public IEnumerable<WeightInitType> WeightInitTypes => weightInitTypes ??
            (weightInitTypes = Enum.GetValues(typeof(WeightInitType)).ToList<WeightInitType>());

        public float LearningRate
        {
            get { return _trainerParameters.LearningRate; }
            set
            {
                if (_trainerParameters.LearningRate != value)
                {
                    _trainerParameters.LearningRate = value;
                    OnPropertyChanged();
                }
            }
        }
        public float LearningRateChange
        {
            get { return _trainerParameters.LearningRateChange; }
            set
            {
                if (_trainerParameters.LearningRateChange != value)
                {
                    _trainerParameters.LearningRateChange = value;
                    OnPropertyChanged();
                }
            }
        }
        public int EpochCount
        {
            get { return _trainerParameters.Epochs; }
            set
            {
                if (_trainerParameters.Epochs != value)
                {
                    _trainerParameters.Epochs = value;
                    OnPropertyChanged();
                }
            }
        }

        public void SynchronizeModelToVM()
        {
            _netParameters.LayersParameters = LayerParameterVMs.Select(x => x.LayerParameters).ToArray();

            if (AreParametersGlobal)
            {
                foreach (var lp in _netParameters.LayersParameters)
                {
                    lp.IsWithBias = IsWithBias_Global;
                    lp.WeightMin = WeightMin_Global;
                    lp.WeightMax = WeightMax_Global;
                    lp.BiasMin = BiasMin_Global;
                    lp.BiasMax = BiasMax_Global;
                }
            }
        }
        public bool AreParametersGlobal
        {
            get { return areParametersGlobal; }
            set
            {
                if (areParametersGlobal != value)
                {
                    areParametersGlobal = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region RelayCommand

        #region I/O Commands

        public IRelayCommand ImportSamplesCommand
        {
            get
            {
                if (importSamplesCommand == null)
                {
                    importSamplesCommand = new RelayCommand(ImportSamplesCommand_Execute, ImportSamplesCommand_CanExecute);
                }
                return importSamplesCommand;
            }
        }
        void ImportSamplesCommand_Execute(object parameter)
        {
            SampleImportWindow.Show();
        }
        bool ImportSamplesCommand_CanExecute(object parameter)
        {
            return true;
        }
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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save this Template";
            saveFileDialog.DefaultExt = ".txt";
            // saveFileDialog.Filter = "Text| *.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                SynchronizeModelToVM();

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
            //_netParameters.Layers = LayerVMs
            //    .Select(x => x.Layer)
            //    .ToArray();
            //Stream stream = File.Open("temp.dat", FileMode.Create);
            //BinaryFormatter b = new BinaryFormatter();
            //b.Serialize(stream, _netParameters);
            //stream.Close();
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
            // CostType = np.CostType; -> from trainerParameters
            WeightInitType = np.WeightInitType;
            LayerParameterVMs = new ObservableCollection<LayerParametersVM>(
                np.LayersParameters.Select(x => new LayerParametersVM(x)));

            foreach (var layerParameterVM in LayerParameterVMs)
            {
                layerParameterVM.PropertyChanged += LayerParametersVM_PropertyChanged;
            }
        }

        #endregion

        #endregion

        #endregion

        #region OnLayerParametersChanged

        void OnLayerParameterVMsChanged(object sender, NotifyCollectionChangedEventArgs e)
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
            for (int i = 0; i < LayerParameterVMs.Count; i++)
            {
                //LayerParameterVMs.ElementAt(i).Id = i;
                //for (int k = 0; k < LayerParameterVMs.ElementAt(i).Inputs.Count; k++)
                //{
                //    LayerParameterVMs.ElementAt(i).Inputs[k] = 0f;
                //}
            }
        }

        #endregion

        #endregion
    }
}
