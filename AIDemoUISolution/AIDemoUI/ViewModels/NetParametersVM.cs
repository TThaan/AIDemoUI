﻿using AIDemoUI.Commands;
using AIDemoUI.Commands.Async;
using AIDemoUI.FactoriesAndStewards;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        ICommand AddCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand MoveLeftCommand { get; }
        ICommand MoveRightCommand { get; }
        IAsyncCommand UseGlobalParametersCommand { get; }
    }

    public class NetParametersVM : BaseVM, INetParametersVM
    {
        #region fields & ctor

        protected INetParameters _netParameters => _sessionContext.NetParameters;
        protected ITrainerParameters _trainerParameters => _sessionContext.TrainerParameters;
        private IEnumerable<CostType> costTypes;
        private IEnumerable<WeightInitType> weightInitTypes;
        private bool areParametersGlobal, areParametersGlobal_IsCheckboxDisabled;
        private float weightMin_Global, weightMax_Global, biasMin_Global, biasMax_Global;
        private ILayerParametersFactory _layerParametersFactory;

        public NetParametersVM(ISessionContext sessionContext, ISimpleMediator mediator, 
            ILayerParametersVMFactory layerParametersVMFactory, ILayerParametersFactory layerParametersFactory)
            : base(sessionContext, mediator)
        {
            LayerParametersVMFactory = layerParametersVMFactory;
            _layerParametersFactory = layerParametersFactory;

            RegisterEvents();
            DefineCommands();
        }

        #region helpers

        private void RegisterEvents()
        {
            _netParameters.LayerParametersCollection.CollectionChanged += LayerParametersCollection_CollectionChanged;
        }
        private void DefineCommands()
        {
            AddCommand = new SimpleRelayCommand(Add);
            DeleteCommand = new SimpleRelayCommand(Delete);
            MoveLeftCommand = new SimpleRelayCommand(MoveLeft);
            MoveRightCommand = new SimpleRelayCommand(MoveRight);
            UseGlobalParametersCommand = new SimpleAsyncRelayCommand(UseGlobalParametersAsync);
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
        public bool AreParametersGlobal_IsCheckboxDisabled
        {
            get { return areParametersGlobal_IsCheckboxDisabled; }
            set
            {
                if (areParametersGlobal_IsCheckboxDisabled != value)
                {
                    areParametersGlobal_IsCheckboxDisabled = value;
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

        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand MoveLeftCommand { get; private set; }
        public ICommand MoveRightCommand { get; private set; }
        public IAsyncCommand UseGlobalParametersCommand { get; private set; }


        #region Executes and CanExecutes

        private void Add(object parameter)
        {
            ILayerParameters lp = (parameter as ILayerParametersVM).LayerParameters;

            int newLayerId = lp.Id + 1;
            ILayerParameters newLayerParameters = _layerParametersFactory.CloneLayerParameters(lp);
            _netParameters.LayerParametersCollection.Insert(newLayerId, newLayerParameters);
        }
        private void Delete(object parameter)
        {
            ILayerParameters lp = (parameter as ILayerParametersVM).LayerParameters;

            if (_netParameters.LayerParametersCollection.Count > 2)
            {
                _netParameters.LayerParametersCollection.Remove(lp);
            }
        }
        private void MoveLeft(object parameter)
        {
            ILayerParameters lp = (parameter as ILayerParametersVM).LayerParameters;

            int currentLayerId = lp.Id;
            _netParameters.LayerParametersCollection.Move(
                currentLayerId, currentLayerId > 0 ? currentLayerId - 1 : 0);
        }
        private void MoveRight(object parameter)
        {
            ILayerParameters lp = (parameter as ILayerParametersVM).LayerParameters;

            int currentLayerId = lp.Id;
            _netParameters.LayerParametersCollection.Move(
                currentLayerId, currentLayerId < _netParameters.LayerParametersCollection.Count - 1 ? currentLayerId + 1 : 0);
        }
        private async Task UseGlobalParametersAsync(object parameter)
        {
            await Task.Run(() =>
            {
                AreParametersGlobal_IsCheckboxDisabled = true;

                if (AreParametersGlobal)
                {
                    AreParametersGlobal = false;
                    OverrideLocalParameters();
                }
                else
                {
                    // 'Invoke' dispatcher to block this thread until the message box is closed.
                    // (BeginInvoke would only block the ui thread (this thread continued execution).)
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (MessageBox.Show(Application.Current.MainWindow, 
                            "Switching to global parameters will override the current (maybe individual) local values.\n" +
                            "Do you want to proceed?", "....", MessageBoxButton.YesNo)
                        == MessageBoxResult.Yes)
                        {
                            AreParametersGlobal = true;
                            OverrideLocalParameters();
                        }
                    });
                }

                AreParametersGlobal_IsCheckboxDisabled = false;

                void OverrideLocalParameters()
                {
                    foreach (var layerParameters in LayerParametersCollection)
                    {
                        layerParameters.BiasMin = biasMin_Global;
                        layerParameters.BiasMax = biasMax_Global;
                        layerParameters.WeightMin = weightMin_Global;
                        layerParameters.WeightMax = weightMax_Global;
                    }
                    OnAllPropertiesChanged();
                }
            });
        }

        #endregion

        #endregion

        #region events

        private void LayerParametersCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Change each layer's id to it's current position

            for (int i = 0; i < LayerParametersCollection.Count; i++)
            {
                LayerParametersCollection[i].Id = i;
            }

            OnAllPropertiesChanged();
        }
        
        #endregion
    }
}
