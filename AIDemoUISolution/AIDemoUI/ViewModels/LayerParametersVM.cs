using AIDemoUI.Commands;
using AIDemoUI.FactoriesAndStewards;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;

namespace AIDemoUI.ViewModels
{
    // wa LayerParameters & ..VMs as IDisposable?
    public class LayerParametersVM : BaseSubVM
    {
        #region fields & ctor

        private readonly ILayerParametersVMFactory _layerParametersVMFactory;

        // First ctor redundant?
        public LayerParametersVM(ISessionContext sessionContext, ILayerParameters layerParameters, ILayerParametersVMFactory layerParametersVMFactory, SimpleMediator mediator)
            : base(sessionContext, mediator)
        {
            _layerParametersVMFactory = layerParametersVMFactory;
            _mediator.Register("Token: MainWindowVM", LayerParametersVMCallback);
            LayerParameters = layerParameters;
        }
        private void LayerParametersVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

        public LayerParametersVM(ISessionContext sessionContext, ILayerParameters layerParameters, ILayerParametersVMFactory layerParametersVMFactory, SimpleMediator mediator, int id)
            : this(sessionContext, layerParameters, layerParametersVMFactory, mediator)
        {
            Id = id;
            SetDefaultValues();
        }

        #region helpers

        private void SetDefaultValues()
        {
            N = 4;
            WeightMin = -1;
            WeightMax = 1;
            BiasMin = 0;
            BiasMax = 0;
            ActivationType = ActivationType.ReLU;
        }

        #endregion

        #endregion

        #region public

        public ILayerParameters LayerParameters { get; }
        public int Id
        {
            get { return LayerParameters.Id; }
            set
            {
                if (LayerParameters.Id != value)
                {
                    LayerParameters.Id = value;
                    OnPropertyChanged();
                }
            }
        }
        public int N
        {
            get { return LayerParameters.NeuronsPerLayer; }
            set
            {
                if (LayerParameters.NeuronsPerLayer != value)
                {
                    LayerParameters.NeuronsPerLayer = value;                    
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMin
        {
            get { return LayerParameters.WeightMin; }
            set
            {
                if (LayerParameters.WeightMin != value)
                {
                    LayerParameters.WeightMin = value;
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMax
        {
            get { return LayerParameters.WeightMax; }
            set
            {
                if (LayerParameters.WeightMax != value)
                {
                    LayerParameters.WeightMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMin
        {
            get { return LayerParameters.BiasMin; }
            set
            {
                if (LayerParameters.BiasMin != value)
                {
                    LayerParameters.BiasMin = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMax
        {
            get { return LayerParameters.BiasMax; }
            set
            {
                if (LayerParameters.BiasMax != value)
                {
                    LayerParameters.BiasMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public ActivationType ActivationType
        {
            get { return LayerParameters.ActivationType; }
            set
            {
                if (LayerParameters.ActivationType != value)
                {
                    LayerParameters.ActivationType = value;
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
            int newLayerId = Id + 1;
            LayerParametersVM newLayerParametersVM = _layerParametersVMFactory.CreateLayerParametersVM(newLayerId);
            _sessionContext.LayerParametersVMCollection.Insert(newLayerId, newLayerParametersVM);
            AdjustIdsToNewPositions();
        }
        public void Delete(object parameter)
        {
            _sessionContext.LayerParametersVMCollection.Remove(this);
            AdjustIdsToNewPositions();
        }
        public void MoveLeft(object parameter)
        {
            int currentLayerId = Id;
            _sessionContext.LayerParametersVMCollection.Move(
                currentLayerId, currentLayerId > 0 ? currentLayerId - 1 : 0);
            AdjustIdsToNewPositions();
        }
        public void MoveRight(object parameter)
        {
            int currentLayerId = Id;
            _sessionContext.LayerParametersVMCollection.Move(
                currentLayerId, currentLayerId < _sessionContext.LayerParametersVMCollection.Count - 1 ? currentLayerId + 1 : 0);
            AdjustIdsToNewPositions();
        }

        #region helpers

        private void AdjustIdsToNewPositions()
        {
            for (int i = 0; i < _sessionContext.LayerParametersVMCollection.Count; i++)
            {
                _sessionContext.LayerParametersVMCollection[i].Id = i;
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
