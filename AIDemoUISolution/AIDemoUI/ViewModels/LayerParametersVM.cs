using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.Generic;

namespace AIDemoUI.ViewModels
{
    public interface ILayerParametersVM : IBaseVM
    {
        ILayerParameters LayerParameters { get; set; }
        int Id { get; }
        int NeuronsPerLayer { get; set; }
        float BiasMin { get; set; }
        float BiasMax { get; set; }
        float WeightMin { get; set; }
        float WeightMax { get; set; }
        ActivationType ActivationType { get; set; }
        IEnumerable<ActivationType> ActivationTypes { get; }
    }

    // wa LayerParameters & ..VMs as IDisposable?
    public class LayerParametersVM : BaseVM, ILayerParametersVM
    {
        #region fields & ctor

        private IEnumerable<ActivationType> activationTypes;

        public LayerParametersVM(ISessionContext sessionContext, ISimpleMediator mediator)//
            : base(sessionContext, mediator) { }

        #endregion

        #region properties

        public ILayerParameters LayerParameters { get; set; }
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

        #endregion
    }
}