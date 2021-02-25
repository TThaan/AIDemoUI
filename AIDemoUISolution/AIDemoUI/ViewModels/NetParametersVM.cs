using AIDemoUI.Commands;
using AIDemoUI.FactoriesAndStewards;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AIDemoUI.ViewModels
{
    public interface INetParametersVM : IBaseVM
    {
        ObservableCollection<ILayerParameters> LayerParametersCollection { get; }
        ILayerParametersVMFactory LayerParametersVMFactory { get; }
        IEnumerable<CostType> CostTypes { get; }
        IEnumerable<WeightInitType> WeightInitTypes { get; }
        CostType CostType { get; set; }//Selected?
        WeightInitType WeightInitType { get; set; }//Selected?
        bool AreParametersGlobal { get; set; }
        float BiasMin_Global { get; set; }
        float BiasMax_Global { get; set; }
        float WeightMin_Global { get; set; }
        float WeightMax_Global { get; set; }
        int EpochCount { get; set; }
        float LearningRate { get; set; }
        float LearningRateChange { get; set; }
        string FileName { get; set; }

        IRelayCommand AddCommand { get; set; }
        IRelayCommand DeleteCommand { get; set; }
        IRelayCommand MoveLeftCommand { get; set; }
        IRelayCommand MoveRightCommand { get; set; }
        void Add(object parameter);
        void Delete(object parameter);
        void MoveLeft(object parameter);
        void MoveRight(object parameter);
    }

    public class NetParametersVM : BaseSubVM, INetParametersVM
    {
        #region fields, private properties & ctor

        private readonly ISessionContext _sessionContext;
        protected INetParameters _netParameters => _sessionContext.NetParameters;
        protected ITrainerParameters _trainerParameters => _sessionContext.TrainerParameters;
        private IEnumerable<CostType> costTypes;
        private IEnumerable<WeightInitType> weightInitTypes;
        private bool areParametersGlobal, areParametersGlobal_IsEnabled;
        private float weightMin_Global, weightMax_Global, biasMin_Global, biasMax_Global;
        private ILayerParametersFactory _layerParametersFactory;

        public NetParametersVM(ISessionContext sessionContext, ISimpleMediator mediator, ILayerParametersVMFactory layerParametersVMFactory, ILayerParametersFactory layerParametersFactory)
            : base(mediator)
        {
            _sessionContext = sessionContext;

            _netParameters.LayerParametersCollection.CollectionChanged += LayerParametersCollection_CollectionChanged;   // DIC?

            LayerParametersVMFactory = layerParametersVMFactory;
            _layerParametersFactory = layerParametersFactory;

            _mediator.Register("Token: MainWindowVM", NetParametersVMCallback);   // DIC?

            SetDefaultValues();
        }

        #region helpers

        private void NetParametersVMCallback(object obj)
        {
            throw new NotImplementedException();
        }
        private void SetDefaultValues()
        {
            _netParameters.LayerParametersCollection.Add(_layerParametersFactory.CreateLayerParameters());
            _netParameters.LayerParametersCollection.Add(_layerParametersFactory.CreateLayerParameters());
            AreParametersGlobal = true;
            AreParametersGlobal_IsEnabled = true;
        }

        #endregion

        #endregion

        #region public

        public ObservableCollection<ILayerParameters> LayerParametersCollection => _sessionContext.NetParameters.LayerParametersCollection;
        public ILayerParametersVMFactory LayerParametersVMFactory { get; }
        public string FileName
        {
            get { return _netParameters.FileName; }
            set
            {
                if (_netParameters.FileName != value)
                {
                    _netParameters.FileName = value;
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
        public bool AreParametersGlobal_IsEnabled// => UseGlobalParametersCommand.CanExecute(null);
        {
            get { return areParametersGlobal_IsEnabled; }
            set
            {
                if (areParametersGlobal_IsEnabled != value)
                {
                    areParametersGlobal_IsEnabled = value;
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

        #endregion

        #region Commands

        public IRelayCommand AddCommand { get; set; }
        public IRelayCommand DeleteCommand { get; set; }
        public IRelayCommand MoveLeftCommand { get; set; }
        public IRelayCommand MoveRightCommand { get; set; }
        public IAsyncCommand UseGlobalParametersCommand { get; set; }


        #region Executes and CanExecutes

        public void Add(object parameter)
        {
            ILayerParameters lp = (parameter as ILayerParametersVM).LayerParameters;

            int newLayerId = lp.Id + 1;
            ILayerParameters newLayerParameters = _layerParametersFactory.CloneLayerParameters(lp);
            _netParameters.LayerParametersCollection.Insert(newLayerId, newLayerParameters);
        }
        public void Delete(object parameter)
        {
            ILayerParameters lp = (parameter as ILayerParametersVM).LayerParameters;

            if (_netParameters.LayerParametersCollection.Count > 2)
            {
                _netParameters.LayerParametersCollection.Remove(lp);
            }
        }
        public void MoveLeft(object parameter)
        {
            ILayerParameters lp = (parameter as ILayerParametersVM).LayerParameters;

            int currentLayerId = lp.Id;
            _netParameters.LayerParametersCollection.Move(
                currentLayerId, currentLayerId > 0 ? currentLayerId - 1 : 0);
        }
        public void MoveRight(object parameter)
        {
            ILayerParameters lp = (parameter as ILayerParametersVM).LayerParameters;

            int currentLayerId = lp.Id;
            _netParameters.LayerParametersCollection.Move(
                currentLayerId, currentLayerId < _netParameters.LayerParametersCollection.Count - 1 ? currentLayerId + 1 : 0);
        }
        public async Task UseGlobalParametersAsync(object parameter)
        {
            await Task.Run(() =>
            {
                AreParametersGlobal_IsEnabled = false;

                if (AreParametersGlobal)
                {
                    AreParametersGlobal = false;
                    OverrideLocalParameters();
                }
                else
                {
                    // Invoke: block thread until message box is dismissed.
                    // BeginInvoke: only block ui thread (this thread continues execution).
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        if (MessageBox.Show(Application.Current.MainWindow, 
                            "Switching to global parameters will override the current (maybe individual) local values.\n" +
                            "Do you want to proceed?", "....", MessageBoxButton.YesNo)
                        == MessageBoxResult.Yes)
                        {
                            AreParametersGlobal = true;
                            OverrideLocalParameters();
                        }
                    }));
                }
                void OverrideLocalParameters()
                {
                    foreach (var layerParameters in LayerParametersCollection)
                    {
                        layerParameters.BiasMin = biasMin_Global;
                        layerParameters.BiasMax = biasMax_Global;
                        layerParameters.WeightMin = weightMin_Global;
                        layerParameters.WeightMax = weightMax_Global;
                    }
                    areParametersGlobal_IsEnabled = true;
                    OnAllPropertiesChanged();
                }
            });
        }
        public bool UseGlobalParametersAsync_CanExecute(object parameter)
        {
            return true;
        }

        #endregion

        #endregion

        #region events

        private void LayerParametersCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < LayerParametersCollection.Count; i++)
            {
                LayerParametersCollection[i].Id = i;
            }
            OnAllPropertiesChanged();
        }
        
        #endregion
    }
}
