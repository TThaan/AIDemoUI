using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI.ViewModels
{
    public class LayerParametersVM : BaseVM
    {
        #region ctor & fields

        public LayerParametersVM(int id)
        {
            LayerParameters = new LayerParameters();
            Id = id;
            SetDefaultValues();
        }

        #region helpers

        void SetDefaultValues()
        {
            IsWithBias = false;
            N = 4;
            WeightMin = -1;
            WeightMax = 1;
            BiasMin = -1;
            BiasMax = 1;
            ActivationType = ActivationType.ReLU;

            // Inputs = Enumerable.Range(0, N).Select(x => 0f).ToObservableCollection();
            // Outputs = Enumerable.Range(0, N).Select(x => 0f).ToObservableCollection();
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
        public bool IsWithBias
        {
            get { return LayerParameters.IsWithBias; }
            set
            {
                if (LayerParameters.IsWithBias != value)
                {
                    LayerParameters.IsWithBias = value;
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
    }
}
