using MatrixHelper;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AIDemoUI.ViewModels
{
    public interface ILayerParametersVM
    {
        // Parameters Data
        ILayerParameters LayerParameters { get; set; }
        int Id { get; }
        int NeuronsPerLayer { get; set; }
        float BiasMin { get; set; }
        float BiasMax { get; set; }
        float WeightMin { get; set; }
        float WeightMax { get; set; }
        ActivationType ActivationType { get; set; }
        IEnumerable<ActivationType> ActivationTypes { get; }

        // Layer Data
        // IMatrix Input { get; }
        // IMatrix Output { get; }
        // IMatrix Biases { get; }
        // IMatrix Weights { get; }
        void Trainer_PropertyChanged(object sender, PropertyChangedEventArgs e);
    }

    // wa LayerParameters & ..VMs as IDisposable?
    public class LayerParametersVM : BaseSubVM, ILayerParametersVM
    {
        #region fields & ctor

        private readonly ISessionContext _sessionContext;
        private IEnumerable<ActivationType> activationTypes;
        //INetParameters _netParameters => _sessionContext.NetParameters;

        public LayerParametersVM(ISessionContext sessionContext, ISimpleMediator mediator)//
            : base(mediator)
        {
            _sessionContext = sessionContext;
            _mediator.Register("Token: MainWindowVM", LayerParametersVMCallback);
        }
        private void LayerParametersVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region properties

        // Parameters data
        public ILayerParameters LayerParameters { get; set; }//=> _netParameters.LayerParametersCollection.Single(x => x.Id == Id);    // 0 - check?
        public int Id => LayerParameters.Id;
        public int NeuronsPerLayer
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
        public IEnumerable<ActivationType> ActivationTypes => activationTypes ??
            (activationTypes = Enum.GetValues(typeof(ActivationType)).ToList<ActivationType>());

        // Layer data (only defined and notified in "observer/tutor mode". (Otherwise Trainer_PropChgd is not registered.))
        // protected ILayer Layer => _sessionContext.Net.Layers[Id];  // Better define once (after net was built)!?
        // public IMatrix Input => Layer?.Input;
        // public IMatrix Output => Layer?.Output;
        // public IMatrix Biases => Layer?.Biases;
        // public IMatrix Weights => Layer?.Weights;

        #endregion

        #region events

        public void Trainer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #endregion
    }
}