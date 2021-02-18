using AIDemoUI.Commands;
using AIDemoUI.FactoriesAndStewards;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace AIDemoUI.ViewModels
{
    public interface INetParametersVM
    {
        INetParameters NetParameters { get; set; }
        ITrainerParameters TrainerParameters { get; set; }
        ObservableCollection<ILayerParameters> LayerParametersCollection { get; }
        IEnumerable<ActivationType> ActivationTypes { get; }
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
        #region fields & ctor

        IEnumerable<ActivationType> activationTypes;
        IEnumerable<CostType> costTypes;
        IEnumerable<WeightInitType> weightInitTypes;
        bool areParametersGlobal;
        float weightMin_Global, weightMax_Global, biasMin_Global, biasMax_Global;
        ILayerParametersFactory _layerParametersFactory;

        public NetParametersVM(ISimpleMediator mediator,
            INetParameters netParameters, ITrainerParameters trainerParameters,
            ObservableCollection<ILayerParameters> layerParametersCollection, ILayerParametersFactory layerParametersFactory)
            : base(mediator)
        {
            NetParameters = netParameters;
            LayerParametersCollection = layerParametersCollection;
            TrainerParameters = trainerParameters;
            _layerParametersFactory = layerParametersFactory;

            _mediator.Register("Token: MainWindowVM", NetParametersVMCallback);
        }

        private void NetParametersVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region public

        public INetParameters NetParameters { get; set; }
        public ObservableCollection<ILayerParameters> LayerParametersCollection { get; }
        public ITrainerParameters TrainerParameters { get; set; }
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

        #region Commands

        public IRelayCommand AddCommand { get; set; }
        public IRelayCommand DeleteCommand { get; set; }
        public IRelayCommand MoveLeftCommand { get; set; }
        public IRelayCommand MoveRightCommand { get; set; }

        #region Executes and CanExecutes

        public void Add(object parameter)
        {
            ILayerParameters lp = parameter as ILayerParameters;
            if (lp == null) return;

            int newLayerId = lp.Id + 1;
            ILayerParameters newLayerParameters = _layerParametersFactory.CreateLayerParameters();
            newLayerParameters.Id = newLayerId;
            LayerParametersCollection.Insert(newLayerId, newLayerParameters);
            AdjustIdsToNewPositions();
        }
        public void Delete(object parameter)
        {
            ILayerParameters lp = parameter as ILayerParameters;
            if (lp == null) return;

            LayerParametersCollection.Remove(lp);
            AdjustIdsToNewPositions();
        }
        public void MoveLeft(object parameter)
        {
            ILayerParameters lp = parameter as ILayerParameters;
            if (lp == null) return;

            int currentLayerId = lp.Id;
            LayerParametersCollection.Move(
                currentLayerId, currentLayerId > 0 ? currentLayerId - 1 : 0);
            AdjustIdsToNewPositions();
        }
        public void MoveRight(object parameter)
        {
            ILayerParameters lp = parameter as ILayerParameters;
            if (lp == null) return;

            int currentLayerId = lp.Id;
            LayerParametersCollection.Move(
                currentLayerId, currentLayerId < LayerParametersCollection.Count - 1 ? currentLayerId + 1 : 0);
            AdjustIdsToNewPositions();
        }

        #region helpers

        private void AdjustIdsToNewPositions()
        {
            for (int i = 0; i < LayerParametersCollection.Count; i++)
            {
                LayerParametersCollection[i].Id = i;
            }
        }

        #endregion

        #endregion

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

        #region helpers

        /// <summary>
        /// Only the model's (NetParameters, LayerParameters) values get adapted to the global/local values.
        /// Their view models keep the values set by the user (in the UI) until the net gets created.
        /// </summary>
        void SwitchBetweenGlobalAndLocalParameters()
        {
            if (LayerParametersCollection == null) return;

            if (AreParametersGlobal)
            {
                foreach (var layerParams in LayerParametersCollection)
                {
                    layerParams.WeightMin = WeightMin_Global;
                    layerParams.WeightMax = WeightMax_Global;
                    layerParams.BiasMin = BiasMin_Global;
                    layerParams.BiasMax = BiasMax_Global;
                }
            }
            else
            {
                foreach (var layerParams in LayerParametersCollection)
                {
                    layerParams.WeightMin = layerParams.WeightMin;
                    layerParams.WeightMax = layerParams.WeightMax;
                    layerParams.BiasMin = layerParams.BiasMin;
                    layerParams.BiasMax = layerParams.BiasMax;
                }
            }
        }

        #endregion

        #endregion
    }
}
