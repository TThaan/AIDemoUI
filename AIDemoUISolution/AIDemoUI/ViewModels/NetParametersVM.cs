using AIDemoUI.Commands;
using DeepLearningDataProvider;
using Microsoft.Win32;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public delegate Task OkBtnEventHandler(INetParameters netParameters, bool isTurnBased, SampleSetParameters sampleSetParameters);

    public class NetParametersVM : BaseSubVM
    {
        #region fields & ctor

        IRelayCommand saveCreatedNetCommand;
        IAsyncCommand loadNetCommandAsync, saveNetCommandAsync, loadInitializedNetCommandAsync;
        IEnumerable<ActivationType> activationTypes;
        IEnumerable<CostType> costTypes;
        IEnumerable<WeightInitType> weightInitTypes;
        ObservableCollection<LayerParametersVM> layerParameterVMs;
        bool areParametersGlobal;
        float weightMin_Global, weightMax_Global, biasMin_Global, biasMax_Global;
        // FileInfo fullyInitializedNetFileInfo;

        public NetParametersVM(MainWindowVM mainVM)
            : base(mainVM)
        {
            NetParameters = new NetParameters();
            TrainerParameters = new TrainerParameters();
            
            SetDefaultValues();
        }

        #region helpers
        // In parameters class ?
        void SetDefaultValues()
        {
            AreParametersGlobal = true;
            WeightMin_Global = -1;
            WeightMax_Global = 1;
            BiasMin_Global = 0;
            BiasMax_Global = 0;
            CostType = CostType.SquaredMeanError;
            WeightInitType = WeightInitType.Xavier;

            LayerParameterVMs = new ObservableCollection<LayerParametersVM>(); 
            LayerParameterVMs.CollectionChanged += LayerParameterVMs_CollectionChanged;
            LayerParameterVMs.Add(new LayerParametersVM(_mainVM, 0));
            LayerParameterVMs.Add(new LayerParametersVM(_mainVM, 1));
            LayerParameterVMs.Add(new LayerParametersVM(_mainVM, 2));
            LayerParameterVMs.Add(new LayerParametersVM(_mainVM, 3));
            LayerParameterVMs.Add(new LayerParametersVM(_mainVM, 4));

            LearningRate = .1f;
            LearningRateChange = .9f;
            EpochCount = 10;
        }

        #endregion

        #endregion

        #region public

        public INetParameters NetParameters { get; set; }
        public ITrainerParameters TrainerParameters { get; set; }
        public NetCreationMode Mode
        {
            get 
            {
                return NetParameters.Mode;
            }
            set
            {
                if (NetParameters.Mode != value)
                {
                    NetParameters.Mode = value;
                    OnPropertyChanged();
                }
            }
        }
        public string FileName
        {
            get { return NetParameters.FileName; }
            set
            {
                if (NetParameters.FileName != value)
                {
                    NetParameters.FileName = value;
                    OnPropertyChanged();
                }
            }
        }
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
            get { return TrainerParameters.CostType; }
            set
            {
                if (TrainerParameters.CostType != value)
                {
                    TrainerParameters.CostType = value;
                    OnPropertyChanged();
                }
            }
        }
        public WeightInitType WeightInitType
        {
            get { return NetParameters.WeightInitType; }
            set
            {
                if (NetParameters.WeightInitType != value)
                {
                    NetParameters.WeightInitType = value;
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
            get { return TrainerParameters.LearningRate; }
            set
            {
                if (TrainerParameters.LearningRate != value)
                {
                    TrainerParameters.LearningRate = value;
                    OnPropertyChanged();
                }
            }
        }
        public float LearningRateChange
        {
            get { return TrainerParameters.LearningRateChange; }
            set
            {
                if (TrainerParameters.LearningRateChange != value)
                {
                    TrainerParameters.LearningRateChange = value;
                    OnPropertyChanged();
                }
            }
        }
        public int EpochCount
        {
            get { return TrainerParameters.Epochs; }
            set
            {
                if (TrainerParameters.Epochs != value)
                {
                    TrainerParameters.Epochs = value;
                    OnPropertyChanged();
                }
            }
        }

        
        #endregion

        #region RelayCommand

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
                    SerializedParameters sp = (SerializedParameters)b.Deserialize(stream);
                    SetLoadedValues(sp);
                });
            }
        }
        bool LoadNetCommand_CanExecute(object parameter)
        {
            return true;
        }
        // Rename to SaveParametersCommandAsync!
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
                SwitchBetweenGlobalAndLocalParameters();
                SerializedParameters sp = new SerializedParameters()
                {
                    NetParameters = NetParameters,
                    TrainerParameters = TrainerParameters
            };

                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    await Task.Run(() =>
                    {
                        Stream stream = saveFileDialog.OpenFile();
                        BinaryFormatter b = new BinaryFormatter();
                        b.Serialize(stream, sp);
                        stream.Close();
                    });
                }
            }
        }
        bool SaveNetCommand_CanExecute(object parameter)
        {
            return true;
        }

        public IAsyncCommand LoadInitializedNetCommandAsync
        {
            get
            {
                if (loadInitializedNetCommandAsync == null)
                {
                    loadInitializedNetCommandAsync = new AsyncRelayCommand(LoadInitializedNetCommand_Execute, LoadInitializedNetCommand_CanExecute);
                }
                return loadInitializedNetCommandAsync;
            }
        }
        async Task LoadInitializedNetCommand_Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    // FullyInitializedNetFileInfo = new FileInfo(openFileDialog.FileName);
                    FileName = openFileDialog.FileName;
                    Mode = NetCreationMode.Load;
                    // Deactivate netParameters options
                });
            }
        }
        bool LoadInitializedNetCommand_CanExecute(object parameter)
        {
            return true;
        }

        public IRelayCommand SaveCreatedNetCommand
        {
            get
            {
                if (saveCreatedNetCommand == null)
                {
                    saveCreatedNetCommand = new RelayCommand(SaveCreatedNetCommand_Execute, SaveCreatedNetCommand_CanExecute);
                }
                return saveCreatedNetCommand;
            }
        }
        void SaveCreatedNetCommand_Execute(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Choose the location to save your newly created net at.";
            saveFileDialog.DefaultExt = ".dat";
            // saveFileDialog.Filter = "Text| *.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                // FullyInitializedNetFileInfo = new FileInfo(saveFileDialog.FileName);    // only before initializing?
                FileName = saveFileDialog.FileName;
                Mode = NetCreationMode.CreateAndSave;
            }
        }
        bool SaveCreatedNetCommand_CanExecute(object parameter)
        {
            return true;
        }
        
        #region helpers

        void SetLoadedValues(SerializedParameters sp)
        {
            NetParameters = sp.NetParameters;
            LayerParameterVMs = new ObservableCollection<LayerParametersVM>(
                sp.NetParameters.LayersParameters.Select(x => new LayerParametersVM(_mainVM, x)));
            TrainerParameters = sp.TrainerParameters;

            // After loading serialized parameters each local property gets set to it's stored value.
            // So the value of AreParametersGlobal can be set to false here by default
            // and the global properties can be set to the parameters of the first layer.
            // This way they are correct in the case of AreParametersGlobal being true
            // whereas they get ignored otherwise.
            AreParametersGlobal = false;
            WeightMin_Global = sp.NetParameters.LayersParameters[0].WeightMin;
            WeightMax_Global = sp.NetParameters.LayersParameters[0].WeightMax;
            BiasMin_Global = sp.NetParameters.LayersParameters[0].BiasMin;
            BiasMax_Global = sp.NetParameters.LayersParameters[0].BiasMax;

            // Notify the UI (via properties in NetParametersVM  and LayerParametersVMs)
            // about changed values in NetParameters and LayerParameters.
            OnAllPropertiesChanged();
        }

        #endregion

        #endregion

        #region LayerParametersVM_CollectionChanged

        private void LayerParameterVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NetParameters.LayersParameters = LayerParameterVMs.Select(x => x.LayerParameters).ToArray();
        }

        #endregion

        #region INotifyPropertyChanged

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(AreParametersGlobal))
            {
                SwitchBetweenGlobalAndLocalParameters();
            }

            base.OnPropertyChanged(propertyName);
        }

        #region

        /// <summary>
        /// Only the model's (NetParameters, LayerParameters) values get adapted to the global/local values.
        /// Their view models keep the values set by the user (in the UI) until the net gets created.
        /// </summary>
        void SwitchBetweenGlobalAndLocalParameters()
        {
            if (LayerParameterVMs == null) return;

            if (AreParametersGlobal)
            {
                foreach (var layerVM in LayerParameterVMs)
                {
                    layerVM.LayerParameters.WeightMin = WeightMin_Global;
                    layerVM.LayerParameters.WeightMax = WeightMax_Global;
                    layerVM.LayerParameters.BiasMin = BiasMin_Global;
                    layerVM.LayerParameters.BiasMax = BiasMax_Global;
                }
            }
            else
            {
                foreach (var layerVM in LayerParameterVMs)
                {
                    layerVM.LayerParameters.WeightMin = layerVM.WeightMin;
                    layerVM.LayerParameters.WeightMax = layerVM.WeightMax;
                    layerVM.LayerParameters.BiasMin = layerVM.BiasMin;
                    layerVM.LayerParameters.BiasMax = layerVM.BiasMax;
                }
            }
        }

        #endregion
        #endregion
    }
}
