using AIDemoUI.Factories;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public delegate Task OkBtnEventHandler(INetParameters netParameters, bool isTurnBased, SampleSetParameters sampleSetParameters);

    public class NetParametersVM : BaseSubVM
    {
        #region fields & ctor

        IEnumerable<ActivationType> activationTypes;
        IEnumerable<CostType> costTypes;
        IEnumerable<WeightInitType> weightInitTypes;
        bool areParametersGlobal;
        float weightMin_Global, weightMax_Global, biasMin_Global, biasMax_Global;

        public NetParametersVM(ISessionContext sessionContext, SimpleMediator mediator, INetParameters netParameters, ITrainerParameters trainerParameters, LayerParametersVMFactory layerParametersVMFactory, ObservableCollection<LayerParametersVM> layerParametersVMCollection)//ILayerParametersVM
            : base(sessionContext, mediator)
        {
            NetParameters = netParameters;
            TrainerParameters = trainerParameters;
            LayerParametersVMFactory = layerParametersVMFactory;
            LayerParametersVMCollection = layerParametersVMCollection;

            _mediator.Register("Token: MainWindowVM", NetParametersVMCallback);

            SetDefaultValues();
        }

        private void NetParametersVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

        #region helpers
        // In parameters class ?
        internal void SetDefaultValues()
        {
            AreParametersGlobal = true;
            WeightMin_Global = -1;
            WeightMax_Global = 1;
            BiasMin_Global = 0;
            BiasMax_Global = 0;
            CostType = CostType.SquaredMeanError;
            WeightInitType = WeightInitType.Xavier;

            LayerParametersVMCollection.Add(LayerParametersVMFactory.CreateLayerParametersVM(0));
            LayerParametersVMCollection.Add(LayerParametersVMFactory.CreateLayerParametersVM(1));
            LayerParametersVMCollection.Add(LayerParametersVMFactory.CreateLayerParametersVM(2));
            LayerParametersVMCollection.Add(LayerParametersVMFactory.CreateLayerParametersVM(3));
            LayerParametersVMCollection.Add(LayerParametersVMFactory.CreateLayerParametersVM(4));

            LearningRate = .1f;
            LearningRateChange = .9f;
            EpochCount = 10;
        }

        #endregion

        #endregion

        #region public

        public INetParameters NetParameters { get; set; }
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
        public LayerParametersVMFactory LayerParametersVMFactory { get; }
        public ObservableCollection<LayerParametersVM> LayerParametersVMCollection { get; }
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
            if (LayerParametersVMCollection == null) return;

            if (AreParametersGlobal)
            {
                foreach (var layerVM in LayerParametersVMCollection)
                {
                    layerVM.LayerParameters.WeightMin = WeightMin_Global;
                    layerVM.LayerParameters.WeightMax = WeightMax_Global;
                    layerVM.LayerParameters.BiasMin = BiasMin_Global;
                    layerVM.LayerParameters.BiasMax = BiasMax_Global;
                }
            }
            else
            {
                foreach (var layerVM in LayerParametersVMCollection)
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
